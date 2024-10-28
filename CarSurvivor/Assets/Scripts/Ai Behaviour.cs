using UnityEngine;
using UnityEngine.AI;



public class AiBehaviour : MonoBehaviour
{
    [Header("Variables")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    public bool hasAttacked;

    [Header("States")]
    public float AttackRange;
    public float SpottingRange;
    public bool isInSightRange;
    public bool isInAttackRange;


    private void Awake()
    {
        player = GameManager.Instance.playerTransform.parent;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        isInSightRange = Physics.CheckSphere(transform.position, SpottingRange, whatIsPlayer);
        isInAttackRange = Physics.CheckSphere(transform.position, AttackRange, whatIsPlayer);


        if(!isInSightRange && !isInAttackRange)
        {
            Patrolling();
        }
        if(isInSightRange && !isInAttackRange) 
        {
            ChasingPlayer();
        }
        if (isInSightRange && isInAttackRange)
        {
            AttackPlayer();
        }

    }


    private void Patrolling() 
    {
        if (!walkPointSet) SearchWalkPoint();
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randx, transform.position.y, transform.position.z + randZ);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) 
        {
            walkPointSet = true;
        }
    }

    private void ChasingPlayer() 
    {
        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer() 
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);

        if(!hasAttacked)
        {
            hasAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }
}
