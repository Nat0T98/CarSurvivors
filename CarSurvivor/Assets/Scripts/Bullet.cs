using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; 
    public float lifetime = 5f;
    public bool punchthrough = false;

    void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        if (collision.gameObject.GetComponent<Enemy>() != null)
    //        {
    //            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
    //        }
    //        //else if(collision.gameObject.GetComponent<RunnerEnemy>() != null)
    //        //{
    //        //    collision.gameObject.GetComponent<RunnerEnemy>().TakeDamage(damage);
    //        //}
    //    }
    //    print("bullet hit");
    //    if (punchthrough == false)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        print(other.name + "ksrhbkajserfbnk");
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<Enemy>() != null)
            {
                //other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                GameManager.Instance.player.GetComponent<CarMechanics>().DamageEnemy(other.GetComponent<Enemy>(), damage);
            }
            //else if(collision.gameObject.GetComponent<RunnerEnemy>() != null)
            //{
            //    collision.gameObject.GetComponent<RunnerEnemy>().TakeDamage(damage);
            //}
        }
        print(other.name);
        if (punchthrough == false)
        {
            Destroy(gameObject);
        }
    }
}
