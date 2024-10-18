using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private GameObject PlayerObject;
    public float speed = 1f;
    public float respawnTime = 3f;
    public float maxHealth = 100;
    public float health;

    private Rigidbody rb;
    public Vector3 spawnLoc;
    MainCar car;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerObject = 
        spawnLoc = gameObject.transform.position;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (PlayerObject.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void TakeDamage(float damage)
    {
        PlayerObject.GetComponent<MainCar>().DamageEnemy(gameObject.GetComponent<Enemy>(), damage);
    }

    //public void Damage(float damage)
    //{
    //    health -= damage;
    //    if (health <= 0)
    //    {
    //        StartCoroutine(Death());
    //    }
    //}

    //IEnumerator Death()
    //{
    //    //gameObject.SetActive(false);
    //    print("respawning");
    //    yield return new WaitForSeconds(respawnTime);
    //    transform.position = spawnLoc;
    //    gameObject.SetActive(true);
    //    health = maxHealth;
    //}
}
