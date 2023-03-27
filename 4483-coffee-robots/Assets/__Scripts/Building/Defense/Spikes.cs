using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip attackAudio;
    AudioSource source;
    
    private BuildStatus buildStatus;
    private float lastAttackTime;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 0.7f;
        source.priority = 150;
        
        buildStatus = BuildStatus.Built;
    }

    void OnTriggerStay(Collider other)
    {
        if (Time.time > lastAttackTime + (1/Config.attackRateSpikes) && (other.CompareTag("Enemy") || other.CompareTag("InvisibleEnemy")) && buildStatus == BuildStatus.Built)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Config.damageSpikes, "spikes");
            }

            lastAttackTime = Time.time;

            source.clip = attackAudio;
            source.Play();
        }
    }

    public void DamageBuilding(float damage, string source = "") {}

    public void FixBuilding() {}
}
