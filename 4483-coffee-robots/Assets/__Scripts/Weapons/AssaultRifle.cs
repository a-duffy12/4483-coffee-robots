using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssaultRifle : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject altPrefab;
    public string weaponName = "assault_rifle";
    public int weaponInt = 0;

    [Header("Audio")]
    public AudioClip fireAudio;
    public AudioClip emptyAudio;
    public AudioClip altAudio;

    [HideInInspector] public int currentAmmo;
    private float lastFireTime;
    
    AudioSource audioSource;
    Camera mainCamera;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
                IEnemy enemy = hit.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageAR, weaponName);
                }
            }

            currentAmmo--;
            lastFireTime = Time.time;

            audioSource.clip = fireAudio;
            audioSource.Play();

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            muzzleFlash.Play();
        }
        else if (Time.time > (lastFireTime + (1/Config.fireRateAR))) // no ammo and can fire
        {
            audioSource.clip = emptyAudio;
            audioSource.Play();
        }
    }

    public void AlternateFire(Transform firePoint)
    {
        if (Config.alternateARUnlocked && Time.time > (lastFireTime + (1/Config.altFireRateAR)))
        {
            GameObject altObject = Instantiate(altPrefab, firePoint.position + (firePoint.forward * 1f), firePoint.rotation);
            Cluster cluster = altObject.GetComponent<Cluster>();
            cluster.Launch((firePoint.transform.forward + firePoint.transform.up), Config.altLaunchForceAR);

            currentAmmo--;
            lastFireTime = Time.time;

            audioSource.clip = altAudio;
            audioSource.Play();

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            muzzleFlash.Play();
        }
        else if (Config.alternateARUnlocked && Time.time > (lastFireTime + (1/Config.altFireRateAR))) // no ammo and can fire
        {
            audioSource.clip = emptyAudio;
            audioSource.Play();
        }
    }

    public void OverrideLastFireTime() // allows weapon to fire as soon as it is swapped to
    {
        if (!Config.alternateARUnlocked)
        {
            lastFireTime = Time.time - (1/Config.fireRateAR);
        }
        else
        {
            lastFireTime = Time.time - (1/Config.altFireRateAR);
        }
    }
}
