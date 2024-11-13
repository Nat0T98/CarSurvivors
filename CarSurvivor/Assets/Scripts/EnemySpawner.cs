using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool Names")]
    public string runnerPoolName = "RunnerPool";
    public string rangedPoolName = "RangedPool"; 

    [Header("Spawn Settings")]
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 20f;
    public float spawnInterval = 5f;
    [Range(0f, 1f)]
    public float runnerProbability = 0.7f;

    private float nextSpawnTime = 0f;
    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.player.transform;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void TrySpawnEnemy()
    {
        if (player == null) return;

        Vector3 randPos = GetRandomSpawnPosition();
        if (IsOnNavMesh(randPos))
        {
            //Debug.Log("Spawning Enemy");
            string poolName = ChooseEnemyType();
            GameObject enemy = ObjectPooler.Instance.SpawnFromPool(poolName, randPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randDir = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnOffset = new Vector3(randDir.x, 0, randDir.y) * distance;
        return player.position + spawnOffset;
    }

    bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas);
    }

    string ChooseEnemyType()
    {
        float chance = Random.value;
        return chance <= runnerProbability ? runnerPoolName : rangedPoolName;
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, minSpawnRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, maxSpawnRadius);
        }
    }
}
