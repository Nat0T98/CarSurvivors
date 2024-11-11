using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public GameObject oilPrefab;
    public int oilSpawnCountMin = 1;
    public int oilSpawnCountMax = 3;
    [HideInInspector]public float health;
    [SerializeField]
    private float lifetime = 10f;  // Lifetime of the enemy in seconds
    private float lifetimeTimer;
    private UpgradeManager upgradeManager;
    private static bool isQuitting = false;
   
    void Start()
    {
        health = maxHealth;
        lifetimeTimer = lifetime;
    }
    void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        //PlayerObject.GetComponent<MainCar>().DamageEnemy(gameObject.GetComponent<Enemy>(), damage);
        health -= damage;
        if (health <= 0 )
        {
            /* health = 0;
             upgradeManager.AddUpgradePoints();*/
            SFX_Manager.GlobalSFXManager.PlaySFX("Drone_Death");
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (isQuitting) return;

        int spawnCount = Random.Range(oilSpawnCountMin, oilSpawnCountMax);
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(oilPrefab, transform.position, Quaternion.identity);
        }
    }

}
