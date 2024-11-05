using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    [HideInInspector]public float health;
    [SerializeField]
    private float lifetime = 10f;  // Lifetime of the enemy in seconds
    private float lifetimeTimer;
    private UpgradeManager upgradeManager;
   
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
            Destroy(gameObject);
        }
    }
    
}
