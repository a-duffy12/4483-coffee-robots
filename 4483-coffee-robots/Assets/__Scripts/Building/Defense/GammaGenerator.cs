using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GammaGenerator : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private List<LineRenderer> lineRenderers;

    //[Header("Audio")]
    //public AudioClip attackAudio;

    private BuildStatus buildStatus;

    AudioSource source;
    private float lastAttackTime;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 0.7f;
        source.priority = 150;

        buildStatus = BuildStatus.Built;
    }

    void Update()
    {
        if (buildStatus == BuildStatus.Built && Time.time >= lastAttackTime + (1/Config.attackRateGG))
        {
            Attack();
        }

        if (Time.time >= lastAttackTime + 0.15f)
        {
            foreach(LineRenderer lr in lineRenderers)
            {
                lr.enabled = false;
            }
        }
    }

    void Attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<Tuple<float, GameObject>> distanceEnemies = new List<Tuple<float, GameObject>>();
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distanceToEnemy <= Config.rangeGG)
            {
                distanceEnemies.Add(new Tuple<float, GameObject>(distanceToEnemy, enemy));
            }
        }

        distanceEnemies = distanceEnemies.OrderBy(de => de.Item1).Take(Config.targetCountGG).ToList();

        int index = 0;
        foreach(Tuple<float, GameObject> target in distanceEnemies)
        {
            if (Physics.Raycast(firePoint.position, target.Item2.transform.position - firePoint.position, out RaycastHit hit, Config.rangeGG, hitMask))
            {
                IEnemy enemy = hit.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageGG, "gamma_generator");

                    lineRenderers[index].enabled = true;
                    lineRenderers[index].SetPosition(0, firePoint.position);
                    lineRenderers[index].SetPosition(1, hit.collider.gameObject.transform.position + new Vector3(0, 1, 0));
                }
            }

            index++;
        }

        lastAttackTime = Time.time;

        if (distanceEnemies.Any())
        {
            //audioSource.clip = attackAudio;
            //audioSource.Play();
        }
    }
}
