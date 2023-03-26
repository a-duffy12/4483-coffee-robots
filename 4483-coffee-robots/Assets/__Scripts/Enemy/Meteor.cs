using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour, IEnemy
{
    [Header("GameObjects")]
    public Image healthBar;
    public GameObject canvas;
    public GameObject meteoritePrefab;
    public Transform firePoint;
    public Transform firePointMG;
    public ParticleSystem attackFlash;
    public ParticleSystem attackFlashMG;
    [SerializeField] private LayerMask hitMask;

    //[Header("Audio")]
    //public AudioClip attackAudio;
    //public AudioClip attackAudioMG;
    //public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }
    [HideInInspector] public bool targetingPlayer = false;

    private float currentHp;
    
    AudioSource source;
    Rigidbody rb;
    GameObject player;
    GameObject coffeePlant;
    CoffeePlant plant;

    private float lastAttackTime;
    private float lastAttackTimeMG;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        coffeePlant = GameObject.FindGameObjectWithTag("CoffeePlant");
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        plant = coffeePlant.GetComponent<CoffeePlant>();
    }

    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 140;

        currentHp = Config.meteorMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.meteorMaxHp, 0, Config.meteorMaxHp);
        canvas.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void Update()
    {
        float distanceToCoffeePlant = Vector3.Distance(coffeePlant.transform.position, transform.position);
        transform.LookAt(coffeePlant.transform.position);

        if (distanceToCoffeePlant <= Config.meteorRange)
        {
            Attack();
        }
        else
        {
            Move(coffeePlant.transform);
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.meteorRangeMG)
        {
            AttackMG(distanceToPlayer);
        }

        canvas.transform.rotation = Quaternion.Euler(45 - transform.rotation.x, 0 - transform.rotation.y, 0 - transform.rotation.z);
    }

    public void DamageEnemy(float damage, string source = "")
    {
        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.meteorMaxHp, 0, Config.meteorMaxHp);

        if (!targetingPlayer && (source == "assault_rifle" || source == "machete" || source == "shotgun"))
        {
            targetingPlayer = true;
        }

        if (currentHp <= 0)
        {

            if (source == "assault_rifle" || source == "machete" || source == "shotgun") // player killed enemy
            {
                PlayerInventory.scrap += (int)(Config.meteorScrapReward * Config.activeKillMod);
                PlayerInventory.electronics += (int)(Config.meteorElectronicsReward  * Config.activeKillMod);
                PlayerInventory.tech += (int)(Config.meteorTechReward  * Config.activeKillMod);
            }
            else
            {
                PlayerInventory.scrap += Config.meteorScrapReward;
                PlayerInventory.electronics += Config.meteorElectronicsReward;
                PlayerInventory.tech += Config.meteorTechReward;
            }
            

            //source.clip = deathAudio;
            //source.Play();

            Destroy(gameObject);
        }
    }

    void Move(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Config.meteorMovementSpeed * Config.difficultyMovementMod * Time.deltaTime);
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + (1/Config.meteorAttackRate))
        {
            lastAttackTime = Time.time;

            //source.clip = attackAudio;
            //source.Play();

            GameObject meteoriteObject = Instantiate(meteoritePrefab, firePoint.position, firePoint.rotation);

            Meteorite meteorite = meteoriteObject.GetComponent<Meteorite>();
            meteorite.Shoot(firePoint.transform.forward, Config.meteorProjectileSpeed);
        }
    }

    void AttackMG(float distance)
    {
        if (Time.time > lastAttackTimeMG + (1/Config.meteorAttackRateMG) && distance <= Config.meteorRangeMG)
        {
            if (Physics.Raycast(firePointMG.position, player.transform.position, out RaycastHit hit, Config.meteorRangeMG, hitMask))
            {
                PlayerSystem system = hit.collider.gameObject.GetComponent<PlayerSystem>();
                if (system != null)
                {
                    system.DamagePlayer(Config.meteorDamageMG * Config.difficultyDamageMod, "meteor");
                }
            }

            lastAttackTimeMG = Time.time;

            //source.clip = attackAudioMG;
            //source.Play();

            if (attackFlashMG.isPlaying)
            {
                attackFlashMG.Stop();
            }
            attackFlashMG.Play();
        }
    }
}
