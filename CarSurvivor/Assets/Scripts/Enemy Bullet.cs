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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<MainCar>() != null)
            {
                GameManager.Instance.player.GetComponent<MainCar>().TakeDamage(damage);
            }
        }
    }
}
