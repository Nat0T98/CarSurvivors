using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public bool punchthrough = false;

    private float lifetimeTimer;

    void OnEnable()
    {
        lifetimeTimer = lifetime;
    }

    void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Bullet hit: " + enemy.gameObject.name);
                GameManager.Instance.player.GetComponent<MainCar>().DamageEnemy(enemy, damage);
            }
        }
        if (!punchthrough)
        {
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool("CarBulletPool", gameObject);
    }

}
