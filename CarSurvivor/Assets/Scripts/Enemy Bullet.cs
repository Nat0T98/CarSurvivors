using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 8f;
    public bool hasMissed = false;
    CarMechanics Player;
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
            Player = other.gameObject.GetComponent<CarMechanics>();
            if (Player != null)
            {
                Player.TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }
    }
}
