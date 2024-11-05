using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject runnerPrefab;
    public GameObject rangedPrefab;
    [Space(10)]
   /* [Header("Spawn Points")]
    public List<Transform> spawnPoints;
    [Space(10)]*/
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
            //Debug.Log("Trying to spawn an enemy...");
        }
    }


    void TrySpawnEnemy()
    {
        if (player == null) return;

        Vector3 randPos = GetRandomSpawnPosition();
        if (IsOnNavMesh(randPos))
        {
            GameObject enemyType = ChooseEnemyType();
            Instantiate(enemyType, randPos, Quaternion.identity);
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

    GameObject ChooseEnemyType()
    {
        float chance = Random.value;
        return chance <= runnerProbability ? runnerPrefab : rangedPrefab;
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
