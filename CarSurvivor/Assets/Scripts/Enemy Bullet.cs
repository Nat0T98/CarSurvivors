using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public bool hasMissed = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MainCar player = collision.gameObject.GetComponent<MainCar>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        
    }
}
