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

    [Space(10)]
    [Header("NavMesh References")]
    public NavMeshAgent agent;
    public GameObject player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    private Rigidbody rb;
    private Vector3 Target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameManager.Instance.player;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Target = player.transform.position;

        // Check attack radius
        isInAttackRange = Physics.CheckSphere(transform.position, AttackingRange, whatIsPlayer);
        isInStoppingRange = Physics.CheckSphere(transform.position, StoppingRange, whatIsPlayer);

        if (isInAttackRange && !isInStoppingRange)
        {
            ChasingPlayer();
            AttackPlayer();
        }
        else if (isInStoppingRange)
        {
            rb.velocity = Vector3.zero;
            AttackPlayer();
        }
        else if (!isInAttackRange && !isInStoppingRange)
        {
            ChasingPlayer();
        }
    }

    private void ChasingPlayer()
    {
        transform.LookAt(Target);
        agent.SetDestination(Target);
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
                SFX_Manager.GlobalSFXManager.PlaySFX("Drone_Laser");
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
}
