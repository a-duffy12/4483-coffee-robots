using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash1;
    [SerializeField] private ParticleSystem muzzleFlash2;
    public string weaponName = "shotgun";
    public int weaponInt = 2;

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

        currentAmmo = Config.maxAmmoShot;
    }

    void Update()
    {
        if (currentAmmo > Config.maxAmmoShot)
        {
            currentAmmo = Config.maxAmmoShot;
        }
    }

    public void Fire(Transform firePoint)
    {
        if (Time.time > (lastFireTime + (1/Config.fireRateShot)) && currentAmmo > 0) // has ammo and can fire
        {
            Vector3 downPellet = Quaternion.Euler(-3.0f, 0, 0) * firePoint.transform.forward;
            Vector3 upPellet = Quaternion.Euler(3.0f, 0, 0) * firePoint.transform.forward;
            Vector3 rightPellet = Quaternion.Euler(0, 3.0f, 0) * firePoint.transform.forward;
            Vector3 leftPellet = Quaternion.Euler(0, -3.0f, 0) * firePoint.transform.forward;
            Vector3 downleftPellet = Quaternion.Euler(-2.25f, -2.25f, 0) * firePoint.transform.forward;
            Vector3 upleftPellet = Quaternion.Euler(2.25f, -2.25f, 0) * firePoint.transform.forward;
            Vector3 downrightPellet = Quaternion.Euler(-2.25f, 2.25f, 0) * firePoint.transform.forward;
            Vector3 uprightPellet = Quaternion.Euler(2.25f, 2.25f, 0) * firePoint.transform.forward;

            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit1, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit1.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, downPellet, out RaycastHit hit2, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit2.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, upPellet, out RaycastHit hit3, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit3.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, rightPellet, out RaycastHit hit4, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit4.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, leftPellet, out RaycastHit hit5, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit5.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, downleftPellet, out RaycastHit hit6, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit6.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, upleftPellet, out RaycastHit hit7, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit7.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, downrightPellet, out RaycastHit hit8, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit8.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            if (Physics.Raycast(firePoint.position, uprightPellet, out RaycastHit hit9, Config.rangeShot, hitMask))
            {
                /*Enemy enemy = hit9.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }*/
            }

            currentAmmo--;
            lastFireTime = Time.time;

            //audioSource.clip = fireAudio;
            //audioSource.Play();

            if (muzzleFlash1.isPlaying)
            {
                muzzleFlash1.Stop();
            }
            if (muzzleFlash2.isPlaying)
            {
                muzzleFlash2.Stop();
            }
            muzzleFlash1.Play();
            muzzleFlash2.Play();
        }
        else if (Time.time > (lastFireTime + (1/Config.fireRateShot))) // no ammo and can fire
        {
            //audioSource.clip = emptyAudio;
            //audioSource.Play();
        }
    }

    public void OverrideLastFireTime() // allows weapon to fire as soon as it is swapped to
    {
        lastFireTime = Time.time - (1/Config.fireRateShot);
    }
}
