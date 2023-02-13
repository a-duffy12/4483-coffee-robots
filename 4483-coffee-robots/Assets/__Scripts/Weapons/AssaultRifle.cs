using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    public string weaponName = "assault_rifle";
    public int weaponInt = 0;

    //[Header("Audio")]

    [HideInInspector] public int currentAmmo;
    private float lastFireTime;
    
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

        currentAmmo = Config.maxAmmoAR;
    }

    void Update()
    {
        if (currentAmmo > Config.maxAmmoAR)
        {
            currentAmmo = Config.maxAmmoAR;
        }
    }

    public void Fire(Transform firePoint)
    {
        if (Time.time > (lastFireTime + (1/Config.fireRateAR)) && currentAmmo > 0) // has ammo and can fire
        {
            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit, Config.rangeAR, hitMask))
            {
                /*Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageAR, "weaponName");
                }*/
            }

            currentAmmo--;
            lastFireTime = Time.time;

            //audioSource.clip = fireAudio;
            //audioSource.Play();

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            muzzleFlash.Play();
        }
        else if (Time.time > (lastFireTime + (1/Config.fireRateAR))) // no ammo and can fire
        {
            //audioSource.clip = emptyAudio;
            //audioSource.Play();
        }
    }

    public void AlternateFire(Transform firePoint)
    {
        if (Config.alternateARUnlocked)
        {
            
        }
    }

    public void OverrideLastFireTime() // allows weapon to fire as soon as it is swapped to
    {
        lastFireTime = Time.time - (1/Config.fireRateAR);
    }
}
