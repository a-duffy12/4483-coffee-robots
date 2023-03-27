using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whale : MonoBehaviour, IEnemy
{
    [Header("GameObjects")]
    public Image healthBar;
    public GameObject canvas;

    [Header("Audio")]
    public AudioClip threatAudio;
    public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }

    private float currentHp;

    AudioSource enemySource;
    Rigidbody rb;

    private GameObject target;
    private float checkDefenseTime;

    void Awake()
    {
        enemySource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 140;

        currentHp = Config.whaleMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.whaleMaxHp, 0, Config.whaleMaxHp);
        canvas.transform.rotation = Quaternion.Euler(45, 0, 0);

        AcquireTarget();
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            transform.LookAt(target.transform.position);
            
            if (distanceToTarget > Config.whaleRange)
            {
                Move(target.transform);
            }

            if (Time.time % 20 == 0)
            {
                enemySource.clip = threatAudio;
                enemySource.Play();
            }
        }
        else if (target == null && Time.time >= checkDefenseTime)
        {
            AcquireTarget();
        }

        canvas.transform.rotation = Quaternion.Euler(45 - transform.rotation.x, 0 - transform.rotation.y, 0 - transform.rotation.z);
    }

    public void DamageEnemy(float damage, string source = "")
    {
        if (source == "assault_rifle" || source == "machete" || source == "shotgun")
        {
            damage = damage * 4; // player does 4x damage to whale
        }

        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.whaleMaxHp, 0, Config.whaleMaxHp);

        if (currentHp <= 0)
        {

            if (source == "assault_rifle" || source == "machete" || source == "shotgun") // player killed enemy
            {
                PlayerInventory.scrap += (int)(Config.whaleScrapReward * Config.activeKillMod);
                PlayerInventory.electronics += (int)(Config.whaleElectronicsReward  * Config.activeKillMod);
                PlayerInventory.tech += (int)(Config.whaleTechReward  * Config.activeKillMod);
            }
            else
            {
                PlayerInventory.scrap += Config.whaleScrapReward;
                PlayerInventory.electronics += Config.whaleElectronicsReward;
                PlayerInventory.tech += Config.whaleTechReward;
            }
            

            enemySource.clip = deathAudio;
            enemySource.Play();

            Destroy(gameObject);
        }
    }

    void Move(Transform goal)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal.transform.position, Config.whaleMovementSpeed * Config.difficultyMovementMod * Time.deltaTime);
    }

    void AcquireTarget()
    {
        GameObject[] defenses = GameObject.FindGameObjectsWithTag("Defense");
        float distanceToDefense = 1000;
        foreach (GameObject defense in defenses)
        {
            float tempDistance = Vector3.Distance(defense.transform.position, transform.position);
            if (tempDistance < distanceToDefense)
            {
                distanceToDefense = Vector3.Distance(defense.transform.position, transform.position);
                target = defense;
            }
        }

        checkDefenseTime = Time.time + 10f; // wait 10s between checking again if no defenses are found
    }
}
