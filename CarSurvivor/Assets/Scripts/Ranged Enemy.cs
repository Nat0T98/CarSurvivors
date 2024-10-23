using UnityEngine;
public class RangedEnemy : Enemy
{
    [Header("Targeting References")]
    public float shootingRange = 10f;
    public float followRange = 15f;

    [Space(10)]
    [Header("Shooting References")]
    public GameObject bulletPrefab;
    public Transform firingPos;
    public float bulletSpeed = 20f;
    public float bulletSpread = 0.1f;  
    public float damage = 10f;
    public float fireRate = 1f;
    private float nextShot = 0f;

    private bool isShooting = false;
    private Rigidbody rb;
    //private GameObject Target;
    private GameObject Target;
    void Awake()
    {
        Target = GameManager.Instance.player;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        MoveTowards();
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

   

    public void MoveTowards()
    {
        //Target = GameManager.Instance.player;
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
        transform.LookAt(Target.transform);
    }


    void ShootAtPlayer()
    {
        //Target = GameManager.Instance.player;
        GameObject bullet = Instantiate(bulletPrefab, firingPos.position, firingPos.rotation);
        
        Vector3 directionToPlayer = (Target.transform.position - firingPos.position).normalized;
        Vector3 offset = new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0);
        directionToPlayer += offset;
        directionToPlayer.Normalize();

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = directionToPlayer * bulletSpeed;

        nextShot = Time.time + fireRate;

        EnemyBullet eBulletScript = bullet.GetComponent<EnemyBullet>();
        eBulletScript.damage = damage;
    }
}
