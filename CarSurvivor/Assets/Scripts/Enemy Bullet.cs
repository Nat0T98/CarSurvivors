using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public bool hasMissed = false;


    private void OnEnable()
    {
        Invoke("Deactivate", lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MainCar playerCar = other.gameObject.GetComponent<MainCar>();
            if (playerCar != null)
            {
                playerCar.TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }
    }
}
