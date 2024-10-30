using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject runnerPrefab;
    public GameObject rangedPrefab;
    [Space(10)]
    [Header("Spawn Points")]
    public List<Transform> spawnPoints;
    [Space(10)]
    [Header("Spawn Settings")]
    public float spawnInterval = 5f;
    [Range(0f, 1f)]
    public float runnerProbability = 0.7f;

    private float nextSpawnTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
            //Debug.Log("Trying to spawn an enemy...");
        }
        if (spawnInterval > 0.1f)
        {

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            spawnInterval = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0) 
        {
            Debug.LogWarning("No spawn points available!");
            return;
        }

        int locations = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[locations];
        GameObject enemyType = ChooseEnemyType();
        Instantiate(enemyType, spawnPoint.position, spawnPoint.rotation);
        //Debug.Log("Spawning enemy at: " + spawnPoint.position);

        Enemy enemyScript = enemyType.GetComponent<Enemy>();
       /* if (enemyScript != null)
        {
            enemyScript.SetPlayerObj(GameManager.Instance.player);
            enemyScript.SetPlayerTransform(GameManager.Instance.playerTransform);
        }*/

    }

    GameObject ChooseEnemyType()
    {
        float chance = Random.value;

        if (chance <= runnerProbability)
        {
            return runnerPrefab;
        }
        else
        {
            return rangedPrefab;
        }
    }
}
