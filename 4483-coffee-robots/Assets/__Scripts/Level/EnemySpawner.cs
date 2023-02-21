using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private List<GameObject> stage1Enemies;
    [SerializeField] private List<GameObject> stage2Enemies;
    [SerializeField] private List<GameObject> stage3Enemies;
    [SerializeField] private List<GameObject> stage4Enemies;

    private float lastSpawnTime;
    private Vector3 lastSpawnPos1;
    private Vector3 lastSpawnPos2;
    
    void Awake()
    {

    }

    void Start()
    {
        lastSpawnPos1 = new Vector3(0, 0, 0);
        lastSpawnPos2 = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (Config.gameStage == 1 && Time.time <= Config.stage1Time + Config.stage1Duration && Time.time >= lastSpawnTime + Config.stage1SpawnDelay)
        {
            Vector3 position = GetPosition();
            SpawnEnemy(position, stage1Enemies[Random.Range(0, stage1Enemies.Count)]);
        }
        else if (Config.gameStage == 2 && Time.time <= Config.stage2Time + Config.stage2Duration && Time.time >= lastSpawnTime + Config.stage2SpawnDelay)
        {
            Vector3 position = GetPosition();
            SpawnEnemy(position, stage2Enemies[Random.Range(0, stage2Enemies.Count)]);
        }
        else if (Config.gameStage == 3 && Time.time <= Config.stage3Time + Config.stage3Duration && Time.time >= lastSpawnTime + Config.stage3SpawnDelay)
        {
            Vector3 position = GetPosition();
            SpawnEnemy(position, stage3Enemies[Random.Range(0, stage3Enemies.Count)]);
        }
        else if (Config.gameStage == 4 && Time.time <= Config.stage4Time + Config.stage4Duration && Time.time >= lastSpawnTime + Config.stage4SpawnDelay)
        {
            Vector3 position = GetPosition();
            SpawnEnemy(position, stage4Enemies[Random.Range(0, stage4Enemies.Count)]);
        }
    }

    void SpawnEnemy(Vector3 position, GameObject enemy)
    {
        Instantiate(enemy, position, Random.rotation);
        Debug.Log($"{enemy.name} spawned at ({position.x}, {position.y}, {position.z})");

        lastSpawnPos2 = lastSpawnPos1;
        lastSpawnPos1 = position;
        lastSpawnTime = Time.time;
    }

    Vector3 GetPosition()
    {
        Vector3 position = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
        
        while (position == lastSpawnPos1 || position == lastSpawnPos2)
        {
            position = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
        }

        return position;
    }
}
