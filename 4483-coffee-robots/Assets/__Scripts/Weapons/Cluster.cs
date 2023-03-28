using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private ParticleSystem explosionParticle;

    [Header("Audio")]
    public AudioClip explodeAudio;

    Rigidbody rb;
    AudioSource source;
    float launchTime;
    bool exploded = false;

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
        if (!exploded && transform.position.y < 0)
        {
            Explode();
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;

        source.clip = explodeAudio;
        source.Play();

        Instantiate(explosionParticle, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, Config.altRadiusAR);

        foreach (Collider col in colliders)
        {
            IEnemy enemy = col.gameObject.GetComponent<Collider>().GetComponent<IEnemy>();
            CoffeePlant plant = col.gameObject.GetComponent<Collider>().GetComponent<CoffeePlant>();
            PlayerSystem system = col.gameObject.GetComponent<Collider>().GetComponent<PlayerSystem>();

            if (enemy != null)
            {
                enemy.DamageEnemy(Config.altDamageAR, "assault_rifle");
            }

            if (plant != null)
            {
                plant.DamageBuilding(Config.altDamageAR, "assault_rifle");
            }

            if (system != null)
            {
                system.DamagePlayer(Config.altDamageAR, "assault_rifle");
            }
        }
    
        Destroy(gameObject, source.clip.length);
    }
}
