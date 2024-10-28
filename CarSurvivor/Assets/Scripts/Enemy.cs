using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    protected GameObject PlayerObject;
    protected Transform PlayerTransform;
    //public float speed = 1f;
    //public float respawnTime = 3f;
    public float maxHealth = 100;
    public float health;
    private UpgradeManager upgradeManager;
    //public Vector3 spawnLoc;
   
    // Start is called before the first frame update
    void Start()
    {
        SetPlayerObj(PlayerObject);
        SetPlayerTransform(PlayerTransform);
        //spawnLoc = gameObject.transform.position;
        health = maxHealth;
    }

    private void Update()
    {
        SetPlayerTransform(PlayerTransform);
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

    public void SetPlayerObj(GameObject player)
    {
        player = GameManager.Instance.player;   
    }
    public void SetPlayerTransform(Transform playerTransform)
    {
        playerTransform = GameManager.Instance.playerTransform.transform;
    }
    
}
