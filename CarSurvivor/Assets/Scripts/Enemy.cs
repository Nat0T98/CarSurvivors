using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public GameObject oilPrefab;
    public GameObject limbPrefab;
    public int oilSpawnCountMin = 1;
    public int oilSpawnCountMax = 3;
    public int limbSpawnCountMin = 1;
    public int limbSpawnCountMax = 3;

    public GameObject player;
    private CarPlayer playerScr;

    [HideInInspector] public float health;
    [SerializeField] private float lifetime = 10f;
    private float lifetimeTimer;
    private UpgradeManager upgradeManager;
    private static bool isQuitting = false;
    private bool inititOnce = true;

    private void OnEnable()
    {
        //health = maxHealth;
        lifetimeTimer = lifetime;
        Debug.Log("Enemy respawned with full health: " + health);
    }


    private void Update()
    {
        Debug.Log("Test");
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {

        health -= damage;
        Debug.Log("Enemy took damage. Health remaining: " + health);
        if (health <= 0)
        {
            SFX_Manager.GlobalSFXManager.PlaySFX("Drone_Death", 1);
            gameObject.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {

        isQuitting = true;
    }

    private void OnDisable()
    {
        if (isQuitting) return;

        if (inititOnce)
        {
            player = GameManager.Instance.player;
            playerScr = player.GetComponent<CarPlayer>();
        }
        

        int spawnCount = Random.Range(oilSpawnCountMin, oilSpawnCountMax);
        for (int i = 0; i < spawnCount; i++)
        {
            
            GameObject oil = ObjectPooler.Instance.SpawnFromPool("Oil", transform.position, Quaternion.identity);
           
            //Instantiate(oilPrefab, transform.position, Quaternion.identity);
        }

        int limbSpawnCount = Random.Range(limbSpawnCountMin, limbSpawnCountMax);
        for (int i = 0; i < limbSpawnCount; i++)
        {
            //Instantiate(limbPrefab, transform.position, Quaternion.identity);
            GameObject limb = ObjectPooler.Instance.SpawnFromPool("RobotLimb", transform.position, Quaternion.identity);
        }

        if (inititOnce == false && playerScr.targetedObjects.Contains(gameObject))
        {
            playerScr.targetedObjects.Remove(gameObject);
        }
        inititOnce = false;
    }
}




