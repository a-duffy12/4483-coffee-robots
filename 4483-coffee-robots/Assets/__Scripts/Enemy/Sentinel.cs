using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sentinel : MonoBehaviour, IEnemy
{
    [Header("GameObjects")]
    public Image healthBar;
    public GameObject canvas;
    public ParticleSystem attackFlash;

    [Header("Audio")]
    public AudioClip attackAudio;

    [HideInInspector] public float hp { get { return currentHp; } }
    [HideInInspector] public bool targetingPlayer = false;

    private float currentHp;
    
    AudioSource enemySource;
    Rigidbody rb;
    GameObject player;
    GameObject coffeePlant;
    PlayerSystem system;
    CoffeePlant plant;

    private float lastAttackTime;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        coffeePlant = GameObject.FindGameObjectWithTag("CoffeePlant");
        enemySource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        system = player.GetComponent<PlayerSystem>();
        plant = coffeePlant.GetComponent<CoffeePlant>();
    }

    void Start()
    {
        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 140;

        currentHp = Config.sentinelMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.sentinelMaxHp, 0, Config.sentinelMaxHp);
        canvas.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void Update()
    {
        if (!targetingPlayer) // targeting coffee plant
        {
            float distanceToCoffeePlant = Vector3.Distance(coffeePlant.transform.position, transform.position);
            transform.LookAt(coffeePlant.transform.position);

            if (distanceToCoffeePlant <= Config.sentinelRange)
            {
                Attack(distanceToCoffeePlant);
            }
            else
            {
                Move(coffeePlant.transform);
            }
        }
        else // targeting player
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            transform.LookAt(player.transform.position);

            if (distanceToPlayer <= Config.sentinelRange)
            {
                Attack(distanceToPlayer);
            }
            else if (distanceToPlayer >= Config.loseInterestDistance)
            {
                targetingPlayer = false; // enemy loses interest in attacking player
            }
            else
            {
                Move(player.transform);
            }
        }

        canvas.transform.rotation = Quaternion.Euler(45 - transform.rotation.x, 0 - transform.rotation.y, 0 - transform.rotation.z);
    }

    public void DamageEnemy(float damage, string source = "")
    {
        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.sentinelMaxHp, 0, Config.sentinelMaxHp);

        if (!targetingPlayer && (source == "assault_rifle" || source == "machete" || source == "shotgun"))
        {
            targetingPlayer = true;
        }

        if (currentHp <= 0)
        {

            if (source == "assault_rifle" || source == "machete" || source == "shotgun") // player killed enemy
            {
                PlayerInventory.scrap += (int)(Config.sentinelScrapReward * Config.activeKillMod);
                PlayerInventory.electronics += (int)(Config.sentinelElectronicsReward  * Config.activeKillMod);
                PlayerInventory.tech += (int)(Config.sentinelTechReward  * Config.activeKillMod);
            }
            else
            {
                PlayerInventory.scrap += Config.sentinelScrapReward;
                PlayerInventory.electronics += Config.sentinelElectronicsReward;
                PlayerInventory.tech += Config.sentinelTechReward;
            }
            
            Destroy(gameObject);
        }
    }

    void Move(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Config.sentinelMovementSpeed * Config.difficultyMovementMod * Time.deltaTime);
    }

    void Attack(float distance)
    {
        if (Time.time > lastAttackTime + (1/Config.sentinelAttackRate) && distance <= Config.sentinelRange)
        {
            lastAttackTime = Time.time;

            enemySource.clip = attackAudio;
            enemySource.Play();

            if (attackFlash.isPlaying)
            {
                attackFlash.Stop();
            }
            attackFlash.Play();

            if (!targetingPlayer)
            {
                plant.DamageBuilding(Config.sentinelDamage * Config.difficultyDamageMod, "sentinel");
            }
            else
            {
                system.DamagePlayer(Config.sentinelDamage * Config.difficultyDamageMod, "sentinel");
            }
        }
    }
}
