using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sentinel : MonoBehaviour, IEnemy
{
    [Header("GameObjects")]
    public Image healthBar;
    public GameObject canvas;

    //[Header("Audio")]
    //public AudioClip attackAudio;
    //public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }
    [HideInInspector] public bool targetingPlayer = false;

    private float currentHp;
    
    AudioSource source;
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
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        system = player.GetComponent<PlayerSystem>();
        plant = coffeePlant.GetComponent<CoffeePlant>();
    }

    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 140;

        currentHp = Config.sentinelMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.sentinelMaxHp, 0, Config.sentinelMaxHp);
        canvas.transform.rotation = Quaternion.identity;
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
    }

    public void DamageEnemy(float damage, string source = "")
    {
        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.sentinelMaxHp, 0, Config.sentinelMaxHp);

        if (!targetingPlayer && (source == "assault_rifle" || source == "machete" || source == "shotgun"))
        {
            targetingPlayer = true;
        }

        if (currentHp <= 0)
        {
            PlayerInventory.scrap += Config.sentinelScrapReward;
            PlayerInventory.electronics += Config.sentinelElectronicsReward;
            PlayerInventory.tech += Config.sentinelTechReward;

            //source.clip = deathAudio;
            //source.Play();

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

            //source.clip = attackAudio;
            //source.Play();

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
