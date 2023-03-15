using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private BuildStatus buildStatus;
    private float lastAttackTime;

    void Start()
    {
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
        }
    }

    public void DamageBuilding(float damage, string source = "") {}

    public void FixBuilding() {}
}
