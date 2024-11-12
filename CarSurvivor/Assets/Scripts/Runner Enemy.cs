using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public GameObject RunnerPrefab;

    [Header("NavMesh References")]
    public NavMeshAgent agent;
    public GameObject player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    private Rigidbody rb;
    private Vector3 Target;
    private MainCar playerScript;
   
    private Coroutine attackCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameManager.Instance.player;
        agent = gameObject.GetComponent<NavMeshAgent>();
        playerScript = GameManager.Instance.player.GetComponent<MainCar>();
    }

    private void OnEnable()
    {
        // Reset enemy for re-enable
        isCharging = false;
        agent.isStopped = false;
        RunnerPrefab.SetActive(true);
        ExplosionEffect.SetActive(false);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    void Update()
    {
        Target = player.transform.position;
        isInAttackRange = Physics.CheckSphere(transform.position, AttackingRange, whatIsPlayer);

        // Move towards player, if in attack range, start explosion charge up, else chase player
        if (isInAttackRange && !isCharging)
        {
            attackCoroutine = StartCoroutine(RunnerAttack());
        }
        else if (!isInAttackRange && !isCharging)
        {
            ChaseTarget();
        }
    }

    // Navmesh version of move towards
    public void ChaseTarget()
    {
        transform.LookAt(Target);
        agent.SetDestination(Target);
    }

    private IEnumerator RunnerAttack()
    {
        isCharging = true;
        agent.isStopped = true; // Stop movement during charge-up
        //ChargeUp SFX?
       
        yield return new WaitForSeconds(chargeUpTime);

        // Damage player within radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, whatIsPlayer);
        foreach (Collider collider in hitColliders)
        {
            playerScript = collider.GetComponent<MainCar>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
        }

        RunnerPrefab.SetActive(false);
        ExplosionEffect.SetActive(true);
        SFX_Manager.GlobalSFXManager.PlaySFX("Runner_Explosion");

        yield return new WaitForSeconds(0.3f); 
        gameObject.SetActive(false);  // Deactivate the enemy to return to pool
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
