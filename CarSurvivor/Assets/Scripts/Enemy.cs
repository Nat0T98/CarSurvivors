using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    [HideInInspector]public float health;
    private UpgradeManager upgradeManager;
   
    void Start()
    {
        health = maxHealth;
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
