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

    [Header("Audio")]
    public AudioClip fireAudio;
    public AudioClip emptyAudio;
    public AudioClip altAudio;

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
                IEnemy enemy = hit1.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, downPellet, out RaycastHit hit2, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit2.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, upPellet, out RaycastHit hit3, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit3.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, rightPellet, out RaycastHit hit4, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit4.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, leftPellet, out RaycastHit hit5, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit5.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, downleftPellet, out RaycastHit hit6, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit6.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, upleftPellet, out RaycastHit hit7, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit7.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, downrightPellet, out RaycastHit hit8, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit8.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, uprightPellet, out RaycastHit hit9, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit9.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageShot, weaponName);
                }
            }

            currentAmmo--;
            lastFireTime = Time.time;

            audioSource.clip = fireAudio;
            audioSource.Play();

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
            audioSource.clip = emptyAudio;
            audioSource.Play();
        }
    }

    public void AlternateFire(Transform firePoint)
    {
        if (Config.alternateShotgunUnlocked && Time.time > (lastFireTime + (1/Config.fireRateShot)))
        {
            Vector3 p5 = Quaternion.Euler(0, 5f, 0) * firePoint.transform.forward;
            Vector3 pn5 = Quaternion.Euler(0, -5f, 0) * firePoint.transform.forward;
            Vector3 p10 = Quaternion.Euler(0, 10f, 0) * firePoint.transform.forward;
            Vector3 pn10 = Quaternion.Euler(0, -10f, 0) * firePoint.transform.forward;
            Vector3 p15 = Quaternion.Euler(0, 15f, 0) * firePoint.transform.forward;
            Vector3 pn15 = Quaternion.Euler(0, -15f, 0) * firePoint.transform.forward;
            Vector3 p20 = Quaternion.Euler(0, 20f, 0) * firePoint.transform.forward;
            Vector3 pn20 = Quaternion.Euler(0, -20f, 0) * firePoint.transform.forward;

            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit1, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit1.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit1.collider.transform.position = hit1.collider.transform.position + Vector3.Normalize(new Vector3(hit1.collider.transform.position.x - firePoint.position.x, 0, hit1.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, p5, out RaycastHit hit2, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit2.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit2.collider.transform.position = hit2.collider.transform.position + Vector3.Normalize(new Vector3(hit2.collider.transform.position.x - firePoint.position.x, 0, hit1.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, pn5, out RaycastHit hit3, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit3.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit3.collider.transform.position = hit3.collider.transform.position + Vector3.Normalize(new Vector3(hit3.collider.transform.position.x - firePoint.position.x, 0, hit3.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, p10, out RaycastHit hit4, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit4.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit4.collider.transform.position = hit4.collider.transform.position + Vector3.Normalize(new Vector3(hit4.collider.transform.position.x - firePoint.position.x, 0, hit4.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, pn10, out RaycastHit hit5, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit5.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit5.collider.transform.position = hit5.collider.transform.position + Vector3.Normalize(new Vector3(hit5.collider.transform.position.x - firePoint.position.x, 0, hit5.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, p15, out RaycastHit hit6, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit6.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit6.collider.transform.position = hit6.collider.transform.position + Vector3.Normalize(new Vector3(hit6.collider.transform.position.x - firePoint.position.x, 0, hit6.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, pn15, out RaycastHit hit7, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit7.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit7.collider.transform.position = hit7.collider.transform.position + Vector3.Normalize(new Vector3(hit7.collider.transform.position.x - firePoint.position.x, 0, hit7.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, p20, out RaycastHit hit8, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit8.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit8.collider.transform.position = hit8.collider.transform.position + Vector3.Normalize(new Vector3(hit8.collider.transform.position.x - firePoint.position.x, 0, hit8.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            if (Physics.Raycast(firePoint.position, pn20, out RaycastHit hit9, Config.rangeShot, hitMask))
            {
                IEnemy enemy = hit9.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.altDamageShot, weaponName);
                    
                    hit9.collider.transform.position = hit9.collider.transform.position + Vector3.Normalize(new Vector3(hit9.collider.transform.position.x - firePoint.position.x, 0, hit9.collider.transform.position.z - firePoint.position.z)) * Config.altKnockDistanceShot;
                }
            }

            currentAmmo--;
            lastFireTime = Time.time;

            audioSource.clip = altAudio;
            audioSource.Play();

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
        else if (Config.alternateShotgunUnlocked && Time.time > (lastFireTime + (1/Config.fireRateShot))) // no ammo and can fire
        {
            audioSource.clip = emptyAudio;
            audioSource.Play();
        }
    }

    public void OverrideLastFireTime() // allows weapon to fire as soon as it is swapped to
    {
        lastFireTime = Time.time - (1/Config.fireRateShot);
    }
}
