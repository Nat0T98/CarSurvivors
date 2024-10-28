using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    [Space(10)]
    [Header("Shooting References")]
    public Transform firingPos;
    public GameObject Laser;
    public float LaserSpeed;
    public float damage;     
    public float timeBetweenAttacks;
    public float StoppingRange;
    public float AttackingRange;
    [HideInInspector]public bool isInAttackRange;
    [HideInInspector]public bool isInStoppingRange; 
    [HideInInspector]public bool hasAttacked;
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
        //Constant checking of attack radius
        isInAttackRange = Physics.CheckSphere(transform.position, AttackingRange, whatIsPlayer);
        isInStoppingRange = Physics.CheckSphere(transform.position, StoppingRange, whatIsPlayer);
        
      
        if (isInAttackRange && !isInStoppingRange)
        {   
            ChasingPlayer();
            AttackPlayer();
            
        }
        if(isInStoppingRange)
        {
            rb.velocity = Vector3.zero;
            AttackPlayer();
        }
        if(!isInAttackRange && !isInStoppingRange)
        {
            ChasingPlayer();
        }

    }

    //using navmesh version of 'move towards', Use Nav Mesh Agent to adjust enemy speed and acceleration
    private void ChasingPlayer()
    {
        transform.LookAt(Target);
        agent.SetDestination(Target);
    }   

    private void AttackPlayer()
    {
      
        transform.LookAt(Target);      
 
        if (!hasAttacked)
        {
            Rigidbody rigb = Instantiate(Laser, firingPos.position, Quaternion.identity).GetComponent<Rigidbody>();
            rigb.AddForce(transform.forward * LaserSpeed, ForceMode.Impulse);
        
            EnemyBullet eBulletScript = Laser.GetComponent<EnemyBullet>();
            eBulletScript.damage = damage;

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
        Gizmos.DrawWireSphere (transform.position, AttackingRange);
    }

    /* void Update()
     {
         if (Target != null)
         {
             float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);

             if (distanceToPlayer <= shootingRange)
             {
                 rb.velocity = Vector3.zero;

                 if (Time.time >= nextShot)
                 {
                     Debug.Log("FIRING");
                     ShootAtPlayer();
                 }
             }
             else if (distanceToPlayer > followRange)
             {
                 MoveTowards();
                 isShooting = false;
             }
         }
     } 
    
    
    /*void ShootAtPlayer()
    {
         GameObject bullet = Instantiate(bulletPrefab, firingPos.position, firingPos.rotation);

         Vector3 directionToPlayer = (player.transform.position - firingPos.position).normalized;
         Vector3 offset = new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0);
         directionToPlayer += offset;
         directionToPlayer.Normalize();

         Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
         bulletRb.velocity = directionToPlayer * bulletSpeed;

         //nextShot = Time.time + fireRate;

         EnemyBullet eBulletScript = bullet.GetComponent<EnemyBullet>();
         eBulletScript.damage = damage;
    }*/



    //public void MoveTowards()
    //{
    //    Target = GameManager.Instance.player;
    //    Vector3 direction = (Target.transform.position - transform.position).normalized;
    //    rb.velocity = direction * speed;
    //    transform.LookAt(Target.transform);
    //}


    //
    
    
   
}
