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
            if (other.gameObject.GetComponent<Enemy>() != null)
            {
                
                GameManager.Instance.player.GetComponent<MainCar>().DamageEnemy(other.GetComponent<Enemy>(), damage);
            }
        }

        if (punchthrough == false)
        {
            ReturnToPool(); 
        }
    }
    void ReturnToPool()
    {
        gameObject.SetActive(false); 
    }
}
