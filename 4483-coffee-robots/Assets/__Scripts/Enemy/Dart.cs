using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public float flyTime = 5.0f;

    //[Header("Audio")]
    //public AudioClip hitAudio;

    Rigidbody rb;
    AudioSource source;

    private float shootTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        shootTime = Time.time;

        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;
        source.priority = 130;
    }

    void Update()
    {
        if (Time.time > shootTime + flyTime)        
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter(Collision collision)
    {
        IEnemy enemy = collision.collider.GetComponent<IEnemy>();
        //IBuilding building = collision.collider.GetComponent<IBuilding>();
        PlayerSystem system = collision.collider.GetComponent<PlayerSystem>();

        if (enemy != null)
        {
            enemy.DamageEnemy(Config.assassinDamage * Config.difficultyDamageMod, "assassin");

            //source.clip = hitAudio;
            //source.Play();
        }

        if (system != null)
        {
            system.DamagePlayer(Config.assassinDamage * Config.difficultyDamageMod, "assassin");

            //source.clip = hitAudio;
            //source.Play();
        }

        Destroy(gameObject);
    }
}
