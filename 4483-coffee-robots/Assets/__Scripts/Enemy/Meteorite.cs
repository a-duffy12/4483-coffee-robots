using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (Time.time > shootTime + flyTime)        
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 direction, float force, bool enraged = false)
    {
        rb.AddForce(direction * force);
    }
}
