using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    public string weaponName = "machete";
    public int weaponInt = 1;

    //[Header("Audio")]
    //public AudioClip fireAudio;
    //public AudioClip altAudio;

    private float lastAttackTime;
    
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.5f;
        audioSource.priority = 150;
    }

    public void Fire(Transform firePoint)
    {
        if (Time.time > (lastAttackTime + (1/Config.rateMachete))) // can attack
        {
            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit, Config.rangeMachete, hitMask))
            {
                IEnemy enemy = hit.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageMachete, weaponName);
                }
            }

            lastAttackTime = Time.time;

            //audioSource.clip = fireAudio;
            //audioSource.Play();
        }
    }

    public void AlternateFire(Transform firePoint)
    {
        if (Config.alternateMacheteUnlocked && Time.time > (lastAttackTime + (1/Config.altRateMachete)))
        {
            Collider[] colliders = Physics.OverlapSphere(firePoint.position, Config.rangeMachete);

            foreach (Collider col in colliders)
            {
                IEnemy enemy = col.gameObject.GetComponent<IEnemy>();

                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageMachete, weaponName);
                }
            }

            lastAttackTime = Time.time;

            //audioSource.clip = altAudio;
            //audioSource.Play();
        }
    }

    public void OverrideLastFireTime() // allows weapon to fire as soon as it is swapped to
    {
        lastAttackTime = Time.time - (1/Config.rateMachete);
    }
}
