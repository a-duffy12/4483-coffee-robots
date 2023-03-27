using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnragedPayload : MonoBehaviour, IEnemy
{
    [Header("GameObjects")]
    public Image healthBar;
    public GameObject canvas;
    [SerializeField] private ParticleSystem explosionParticle;

    [Header("Audio")]
    public AudioClip attackAudio;
    public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }

    private float currentHp;

    AudioSource enemySource;
    Rigidbody rb;
    GameObject player;
    GameObject coffeePlant;
    PlayerSystem system;
    CoffeePlant plant;

    private bool delivered = false;

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

        currentHp = Config.payloadMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.payloadMaxHp, 0, Config.payloadMaxHp);
        canvas.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void Update()
    {
        float distanceToCoffeePlant = Vector3.Distance(coffeePlant.transform.position, transform.position);
        transform.LookAt(coffeePlant.transform.position);

        if (!delivered && distanceToCoffeePlant <= Config.payloadRange)
        {
            DeliverPayload();
        }
        else
        {
            Move(coffeePlant.transform);
        }

        canvas.transform.rotation = Quaternion.Euler(45 - transform.rotation.x, 0 - transform.rotation.y, 0 - transform.rotation.z);
    }

    public void DamageEnemy(float damage, string source = "")
    {
        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.payloadMaxHp, 0, Config.payloadMaxHp);

        if (currentHp <= 0)
        {

            if (source == "assault_rifle" || source == "machete" || source == "shotgun") // player killed enemy
            {
                PlayerInventory.scrap += (int)(Config.payloadScrapReward * Config.activeKillMod);
                PlayerInventory.electronics += (int)(Config.payloadElectronicsReward  * Config.activeKillMod);
                PlayerInventory.tech += (int)(Config.payloadTechReward  * Config.activeKillMod);
            }
            else
            {
                PlayerInventory.scrap += Config.payloadScrapReward;
                PlayerInventory.electronics += Config.payloadElectronicsReward;
                PlayerInventory.tech += Config.payloadTechReward;
            }
            

            enemySource.clip = deathAudio;
            enemySource.Play();

            Destroy(gameObject);
        }
    }

    void Move(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Config.enragedPayloadMovementSpeed * Config.difficultyMovementMod * Time.deltaTime);
    }

    void DeliverPayload()
    {
        delivered = true;

        enemySource.clip = attackAudio;
        enemySource.Play();

        Instantiate(explosionParticle, transform.position, transform.rotation);
        
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.payloadRange)
        {
            system.DamagePlayer(Config.enragedPayloadDamage * Config.difficultyDamageMod* 0.2f, "payload");
        }

        plant.DamageBuilding(Config.enragedPayloadDamage * Config.difficultyDamageMod, "payload");
        Destroy(gameObject, enemySource.clip.length);
        //Destroy(gameObject);
    }
}
