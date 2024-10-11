using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainCar : MonoBehaviour
{
    public float accelerationForce = 50f; 
    public float reverseForce = 30f; 
    public float maxSpeed = 20f; 
    public float turnSpeed = 150f;
    public float maxDamage = 120f;
    //public int pointsToUpgrade = 3;
    public GameObject wheelSpinnerL;
    public GameObject wheelSpinnerR;
    public GameObject rammer;

    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;

    private bool firstUpgrade;
    private float upgradeScale;
    private float wheelRadius = 0.38f;
    //public int points;

    private Rigidbody rb;
    public UpgradeManager Upgrades;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //print(rb.velocity.magnitude);
        if (Input.GetKeyDown(KeyCode.R))
        {
            //print("reset");
            SceneManager.LoadScene("Level 1");
        }

    }

    void FixedUpdate()
    {
        HandleMovement();
        SpinWheels();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            print("Damaged Enemy");
            if (other.GetComponent<Enemy>() != null)
            {
                DamageEnemy(other.GetComponent<Enemy>());
            }
        }
    }

    void HandleMovement()
    {
        
        float forwardInput = Input.GetAxis("Vertical"); 
        float turnInput = Input.GetAxis("Horizontal"); 

        
        if (forwardInput > 0)
        {
            rb.AddForce(transform.forward * forwardInput * accelerationForce, ForceMode.Acceleration);
        }
        else if (forwardInput < 0)
        {
            rb.AddForce(transform.forward * forwardInput * reverseForce, ForceMode.Acceleration);
            turnInput = 0 - turnInput;
        }

        
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        
        if (rb.velocity.magnitude > 0.1f) 
        {
            float turn = turnInput * ((rb.velocity.magnitude / maxSpeed) * turnSpeed) * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);

        }
    }

    public void MelleUpgradeTest()
    {
        print("COLLECTED");
        if (firstUpgrade == false)
        {
            wheelSpinnerL.SetActive(true);
            wheelSpinnerR.SetActive(true);
            rammer.SetActive(true);
            firstUpgrade = true;
            upgradeScale = wheelSpinnerL.transform.localScale.x;
        }
        else
        {
            upgradeScale += 0.01f;
            wheelSpinnerL.transform.localScale = new Vector3(upgradeScale, wheelSpinnerL.transform.localScale.y, wheelSpinnerL.transform.localScale.z);
            wheelSpinnerR.transform.localScale = new Vector3(upgradeScale, wheelSpinnerR.transform.localScale.y, wheelSpinnerR.transform.localScale.z);
        }
    }

    void SpinWheels()
    {
        
        float distanceTraveled = rb.velocity.magnitude * Time.deltaTime;
        float rotationAngle = (distanceTraveled / (2 * Mathf.PI * wheelRadius)) * 360f;

        
        RotateWheel(wheel1, rotationAngle);
        RotateWheel(wheel2, rotationAngle);
        RotateWheel(wheel3, rotationAngle);
        RotateWheel(wheel4, rotationAngle);
    }

    void RotateWheel(GameObject wheel, float rotationAngle)
    {
        
        wheel.transform.Rotate(Vector3.right, rotationAngle, Space.Self);
    }

    void DamageEnemy(Enemy enemyScript)
    {
        //other.GetComponent<Enemy>().Damage(60f);

        float damage = rb.velocity.magnitude / maxSpeed * maxDamage;

        enemyScript.health -= damage;
        if (enemyScript.health <= 0)
        {
            StartCoroutine(EnemyDeath(enemyScript));
            
        }
    }

    IEnumerator EnemyDeath(Enemy enemyScript)
    {
        enemyScript.gameObject.SetActive(false);
        print("respawning");
        Upgrades.AddUpgradePoints();
        yield return new WaitForSeconds(enemyScript.respawnTime);
        enemyScript.gameObject.transform.position = enemyScript.spawnLoc;
        enemyScript.gameObject.SetActive(true);
        enemyScript.health = enemyScript.maxHealth;
    }



    //void AddPoints()
    //{
    //    points += 1;
    //    /*if (points >= pointsToUpgrade)
    //    {
            
    //        MelleUpgradeTest();
    //        points = 0;
    //    }*/
    


    //Vector3.SmoothDamp()

}
