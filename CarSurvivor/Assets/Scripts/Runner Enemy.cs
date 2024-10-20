using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemy : Enemy
{
    private GameObject Target;
    private Rigidbody rb;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
       
    }
    void Update()
    {
        MoveTowards();
    }
    public void MoveTowards()
    {
        Target = GameManager.Instance.player;
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
        transform.LookAt(Target.transform);
    }
}
