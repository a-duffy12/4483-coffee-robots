using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    float riseTime = 1f;
    float flyTime = 7f;
    
    [Header("GameObjects")]
    [HideInInspector] public GameObject target;
    [HideInInspector] public Transform risePoint;
    [SerializeField] private ParticleSystem explosionParticle;

    [Header("Audio")]
    public AudioClip explodeAudio;

    Rigidbody rb;
    AudioSource source;
    float launchTime;
    bool impacted = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 130;
        
        launchTime = Time.time;
    }

    void Update()
    {
        if (Time.time < launchTime + flyTime && Time.time < launchTime + riseTime) 
        {
            transform.LookAt(risePoint.position);
            transform.position = Vector3.MoveTowards(transform.position, risePoint.position, Config.rocketRiseSpeed * Time.deltaTime);
        }
        else if (Time.time < launchTime + flyTime && Time.time >= launchTime + riseTime)
        {
            if (target.tag != "Enemy" || target == null)
            {
                Destroy(gameObject);
            }

            transform.LookAt(target.transform.position + new Vector3(0, 1, 0));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z), Config.rocketMissileSpeed * Time.deltaTime);
        }
        else if (!impacted)
        {
            Impact(false);
        }

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (!impacted && distanceToTarget <= 1f)
            {
                Impact(true);
            }
        }  
    }

    void OnCollisionEnter(Collision collision)
    {
        Impact(true);
    }

    void Impact(bool success) // success is if missile hit target enemy
    {
        impacted = true;
        
        if (success)
        {
            IEnemy enemy = target.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Config.damageRocket, "rocket_tower");
            }
        }

        Instantiate(explosionParticle, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, Config.splashRadiusRocket);

        foreach (Collider col in colliders)
        {
            IEnemy enemy = col.gameObject.GetComponent<Collider>().GetComponent<IEnemy>();
            CoffeePlant plant = col.gameObject.GetComponent<Collider>().GetComponent<CoffeePlant>();
            PlayerSystem system = col.gameObject.GetComponent<Collider>().GetComponent<PlayerSystem>();

            if (enemy != null)
            {
                enemy.DamageEnemy(Config.splashDamageRocket, "rocket_tower");
            }

            if (plant != null)
            {
                plant.DamageBuilding(Config.splashDamageRocket, "rocket_tower");
            }

            if (system != null)
            {
                system.DamagePlayer(Config.splashDamageRocket, "rocket_tower");
            }
        }

        source.clip = explodeAudio;
        source.Play();
    
        Destroy(gameObject, source.clip.length);
    }
}
