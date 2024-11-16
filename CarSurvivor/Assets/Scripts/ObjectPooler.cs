using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string poolName; 
    public GameObject prefab;
    public int size;
}

public class ObjectPooler : MonoBehaviour
{
    #region Singleton
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.poolName, objectPool); 
        }
    }

    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning("Pool with name " + poolName + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[poolName].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[poolName].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnToPool(string poolName, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning("Pool with name " + poolName + " doesn't exist");
            return;
        }
        objectToReturn.SetActive(false);
        poolDictionary[poolName].Enqueue(objectToReturn);
    }
}
