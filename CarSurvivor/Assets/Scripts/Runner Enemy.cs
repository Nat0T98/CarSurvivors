using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class RunnerEnemy : Enemy
{   
    [Header("Attack References")]
    public float AttackingRange;
    public float chargeUpTime;
    public float damage;
    public float damageRadius;
    [HideInInspector] public bool isInAttackRange;
    private bool isCharging;
    public GameObject ExplosionEffect;

    [Header("NavMesh References")]
    public NavMeshAgent agent;
    public GameObject player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    private Rigidbody rb;
    private Vector3 Target; 
    private MainCar playerScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameManager.Instance.player;
        agent = gameObject.GetComponent<NavMeshAgent>();
        playerScript = GameManager.Instance.player.GetComponent<MainCar>();

    }
    void Update()
    {
        Target = player.transform.position;
        isInAttackRange = Physics.CheckSphere(transform.position, AttackingRange, whatIsPlayer);


        //Moves towards player, if in attack range, start explosion charge up, else chase player
        if (isInAttackRange && !isCharging)
        {
            StartCoroutine(RunnerAttack());
        }
        else if (!isInAttackRange && !isCharging)
        {
            ChaseTarget();
        }

    }
    //Navmesh version of move towards
    public void ChaseTarget()
    { 
        transform.LookAt(Target);
        agent.SetDestination(Target);
    }

    private IEnumerator RunnerAttack()
    {
        isCharging = true;                   
        agent.isStopped = true;              
       //Charge up SFX and Visuals here

        yield return new WaitForSeconds(chargeUpTime);        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, whatIsPlayer);
        foreach (Collider collider in hitColliders)
        {
            playerScript = collider.GetComponent<MainCar>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage); 
            }
        }
        //Add Explosion sfx, particles and whatever else here
       Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
      
        Destroy(gameObject); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

    }

    /* private void OnTriggerEnter(Collider other)
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
     }*/
}
