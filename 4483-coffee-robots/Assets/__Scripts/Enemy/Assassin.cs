using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assassin : MonoBehaviour, IEnemy
{
    [Header("GameObjects")]
    public Image healthBar;
    public GameObject canvas;
    public GameObject dartPrefab;
    public Transform firePoint;

    //[Header("Audio")]
    //public AudioClip attackAudio;
    //public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }

    private float currentHp;
    
    AudioSource source;
    Rigidbody rb;
    GameObject player;
    PlayerSystem system;

    private float lastAttackTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        system = player.GetComponent<PlayerSystem>();
    }
    
    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 140;

        currentHp = Config.assassinMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.assassinMaxHp, 0, Config.assassinMaxHp);
        canvas.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        if (distanceToPlayer < Config.assassinMinRange || distanceToPlayer > Config.assassinMaxRange)
        {
            Move(player.transform, distanceToPlayer);
        }
        else
        {
            Attack();
        }

        canvas.transform.rotation = Quaternion.Euler(45 - transform.rotation.x, 0 - transform.rotation.y, 0 - transform.rotation.z);
    }

    public void DamageEnemy(float damage, string source = "")
    {
        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.assassinMaxHp, 0, Config.assassinMaxHp);

        if (currentHp <= 0)
        {

            if (source == "assault_rifle" || source == "machete" || source == "shotgun") // player killed enemy
            {
                PlayerInventory.scrap += (int)(Config.assassinScrapReward * Config.activeKillMod);
                PlayerInventory.electronics += (int)(Config.assassinElectronicsReward  * Config.activeKillMod);
                PlayerInventory.tech += (int)(Config.assassinTechReward  * Config.activeKillMod);
            }
            else
            {
                PlayerInventory.scrap += Config.assassinScrapReward;
                PlayerInventory.electronics += Config.assassinElectronicsReward;
                PlayerInventory.tech += Config.assassinTechReward;
            }
            
            //source.clip = deathAudio;
            //source.Play();

            Destroy(gameObject);
        }
    }

    void Move(Transform target, float distance)
    {
        if (distance < Config.assassinMinRange) // move away from player
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Config.assassinMovementSpeed * Config.difficultyMovementMod * Time.deltaTime * -1);
        }
        else if (distance > Config.assassinMaxRange) // move towards player
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Config.assassinMovementSpeed * Config.difficultyMovementMod * Time.deltaTime);
        }
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + (1/Config.assassinAttackRate))
        {
            lastAttackTime = Time.time;

            //source.clip = attackAudio;
            //source.Play();

            GameObject dartObject = Instantiate(dartPrefab, firePoint.position, firePoint.rotation);

            Dart dart = dartObject.GetComponent<Dart>();
            dart.Shoot(firePoint.transform.forward, Config.assassinProjectileSpeed);
        }
    }
}
