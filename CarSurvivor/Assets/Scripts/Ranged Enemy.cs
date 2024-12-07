using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    [Space(10)]
    [Header("Shooting References")]
    public Transform firingPos;
    public float LaserSpeed;
    public float damage;
    public float timeBetweenAttacks;
    public float StoppingRange;
    public float AttackingRange;
    [HideInInspector] public bool isInAttackRange;
    [HideInInspector] public bool isInStoppingRange;
    [HideInInspector] public bool hasAttacked;

    public float propRotSpeed = 1f;
    [Serializable]
    public struct Props
    {
        public GameObject propObj;
    }
    public List<Props> props;


    [Space(10)]
    [Header("NavMesh References")]
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    private Rigidbody rb;
    private Vector3 Target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        health = maxHealth;
        hasAttacked = false;
        if (agent != null)
        {
            agent.isStopped = false;
            agent.ResetPath();
        }

    }


    void Update()
    {
        Target = player.transform.position;

        // Check radius for attack and stopping range
        isInAttackRange = Physics.CheckSphere(transform.position, AttackingRange, whatIsPlayer);
        isInStoppingRange = Physics.CheckSphere(transform.position, StoppingRange, whatIsPlayer);

        // Movement and Attack Control
        if (isInStoppingRange)
        {
            agent.isStopped = true;
            AttackPlayer();
        }
        else
        {
            agent.isStopped = false;
            ChasingPlayer();

            if (isInAttackRange)
            {
                AttackPlayer();
            }
        }

        PropSpin();
    }

    private void ChasingPlayer()
    {
        Vector3 directionToTarget = (Target - transform.position).normalized;
        if (Vector3.Distance(agent.destination, Target) > 0.5f)
        {
            agent.SetDestination(Target);
            transform.forward = directionToTarget;
        }
    }


    private void AttackPlayer()
    {
        transform.LookAt(Target); // Keep aiming at player  

        if (!hasAttacked)
        {
            // Spawn the laser from the pool
            GameObject laser = ObjectPooler.Instance.SpawnFromPool("DroneLaserPool", firingPos.position, Quaternion.identity);
            if (laser != null)
            { 
                SFX_Manager.GlobalSFXManager.PlaySFX("Drone_Laser", 1f);
                Rigidbody rigb = laser.GetComponent<Rigidbody>();
                rigb.velocity = Vector3.zero;
                rigb.AddForce(transform.forward * LaserSpeed, ForceMode.Impulse);

                EnemyBullet eBulletScript = laser.GetComponent<EnemyBullet>();
                eBulletScript.damage = damage;
            }

            hasAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, StoppingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackingRange);
    }

    void PropSpin()
    {
        foreach (var prop in props)
        {
            prop.propObj.transform.Rotate(Vector3.up, propRotSpeed * Time.deltaTime);
        }
    }
}
