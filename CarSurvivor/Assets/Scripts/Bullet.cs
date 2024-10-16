using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; 
    public float lifetime = 5f;
    public bool punchthrough = true;

    void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            if (punchthrough == false)
            {
                Destroy(gameObject);
            }
        }

        if (punchthrough == false)
        {
            Destroy(gameObject);
        }
    }
}
