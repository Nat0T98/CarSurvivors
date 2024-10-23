using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemy : Enemy
{
    private MainCar playerScript;

    private GameObject Target;
    private Rigidbody rb;

    public float timeToAttack = 0.5f;
    public float damage = 5f;
    private bool playerIsInRange;
    private float attackTime;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.Instance.player.GetComponent<MainCar>();
    }
    void Update()
    {
        MoveTowards();
        Attacking();
    }
    public void MoveTowards()
    {
        Target = GameManager.Instance.player;
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
        //transform.LookAt(Target.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = false;
        }
    }

    void Attacking()
    {
        if (playerIsInRange)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= timeToAttack)
            {
                playerScript.TakeDamage(damage);
                attackTime = 0;
            }
        }
        else
        {
            attackTime = 0;
        }
    }
}
