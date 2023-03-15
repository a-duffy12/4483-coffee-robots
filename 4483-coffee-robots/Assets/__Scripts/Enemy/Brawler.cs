using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Brawler : MonoBehaviour, IEnemy
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
    PlayerSystem system;
    PlayerController controller;

    private float lastAttackTime;
    private float lastStatusTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        system = player.GetComponent<PlayerSystem>();
        controller = player.GetComponent<PlayerController>();
    }
    
    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 140;

        currentHp = Config.brawlerMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.brawlerMaxHp, 0, Config.brawlerMaxHp);
        canvas.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        if (targetingPlayer)
        {
            if (distanceToPlayer > Config.brawlerRange)
            {
                Move(player.transform);
            }
            else
            {
                Attack();
            }
        }

        canvas.transform.rotation = Quaternion.Euler(45 - transform.rotation.x, 0 - transform.rotation.y, 0 - transform.rotation.z);
        
        if (Time.time > lastStatusTime + 2f)
        {
            CheckBrawlerStatuses(distanceToPlayer);
        }
    }

    public void DamageEnemy(float damage, string source = "")
    {
        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.brawlerMaxHp, 0, Config.brawlerMaxHp);

        if (currentHp <= 0)
        {

            if (source == "assault_rifle" || source == "machete" || source == "shotgun") // player killed enemy
            {
                PlayerInventory.scrap += (int)(Config.brawlerScrapReward * Config.activeKillMod);
                PlayerInventory.electronics += (int)(Config.brawlerElectronicsReward  * Config.activeKillMod);
                PlayerInventory.tech += (int)(Config.brawlerTechReward  * Config.activeKillMod);
            }
            else
            {
                PlayerInventory.scrap += Config.brawlerScrapReward;
                PlayerInventory.electronics += Config.brawlerElectronicsReward;
                PlayerInventory.tech += Config.brawlerTechReward;
            }
            
            //source.clip = deathAudio;
            //source.Play();

            Destroy(gameObject);
        }
    }

    void Move(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Config.brawlerMovementSpeed * Config.difficultyMovementMod * Time.deltaTime);
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + (1/Config.brawlerAttackRate))
        {
            lastAttackTime = Time.time;

            //source.clip = attackAudio;
            //source.Play();

            system.DamagePlayer(Config.brawlerDamage * Config.difficultyDamageMod, "brawler");
            controller.SlowPlayer(Config.brawlerSlowFactor, Config.brawlerSlowDuration);
        }
    }

    void CheckBrawlerStatuses(float distanceToPlayer)
    {
        if (!targetingPlayer)
        {
            GameObject[] others = GameObject.FindGameObjectsWithTag("Enemy").Where(others => others.GetComponent<Brawler>() != null).ToArray();
            float count = 0;
            foreach (GameObject other in others)
            {
                Brawler b = other.GetComponent<Brawler>();
                if (b.targetingPlayer)
                {
                    count++;
                }
            }

            if (count < 3)
            {
                targetingPlayer = true;
            }
        }
        else
        {
            GameObject[] others = GameObject.FindGameObjectsWithTag("Enemy").Where(others => others.GetComponent<Brawler>() != null).ToArray();
            float count;
            foreach (GameObject other in others)
            {
                float tempDistance = Vector3.Distance(player.transform.position, other.transform.position);
                Brawler b = other.GetComponent<Brawler>();
                if (b.targetingPlayer && tempDistance > distanceToPlayer)
                {
                    b.targetingPlayer = false;
                    targetingPlayer = true;
                    break;
                }
            }
        }

        lastStatusTime = Time.time;
    }
}
