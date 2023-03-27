using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private Transform effectsPoint;
    [SerializeField] private Transform risePoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject missilePrefab;

    [Header("Audio")]
    public AudioClip attackAudio;

    private BuildStatus buildStatus;

    AudioSource source;

    private float lastAttackTime;
    private float retargetStartTime;
    private GameObject target;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 150;

        buildStatus = BuildStatus.Built;
    }

    void Update()
    {
        if (buildStatus == BuildStatus.Built)
        {
            if (target == null && Time.time > retargetStartTime + Config.retargetDelayRocket) // not currently targetting any enemy
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                float distanceToEnemy = Config.rangeRocket;
                foreach (GameObject enemy in enemies)
                {
                    float tempDistance = Vector3.Distance(enemy.transform.position, transform.position);
                    if (tempDistance <= Config.rangeRocket && tempDistance < distanceToEnemy)
                    {
                        distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                        target = enemy;
                    }
                }

                retargetStartTime = Time.time;

                Attack();
            }
            else if (target != null) // already have a target
            {
                retargetStartTime = Time.time;
                
                Attack();
            }

        }
    }

    void Attack()
    {
        float distanceToEnemy = Vector3.Distance(target.transform.position, transform.position);

        if (Time.time >= lastAttackTime + (1/Config.attackRateRocket) && distanceToEnemy <= Config.rangeRocket)
        {
            GameObject missileObject = Instantiate(missilePrefab, effectsPoint.position, Quaternion.identity);
            missileObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            Missile missile = missileObject.GetComponent<Missile>();
            missile.target = target;
            
            lastAttackTime = Time.time;

            source.clip = attackAudio;
            source.Play();

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            muzzleFlash.Play();
        }
    }
}
