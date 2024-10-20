using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MainCar : MonoBehaviour
{
    [Header("Car Stats")]
    public float accelerationForce = 50f; 
    public float reverseForce = 30f; 
    public float maxSpeed = 20f; 
    public float turnSpeed = 150f;
    public float maxDamage = 120f;
    [Space(10)]

    [Header("Upgrade  References")]
    public UpgradeManager Upgrades;
    public GameObject wheelSpinnerL;
    public GameObject wheelSpinnerR;
    public GameObject rammer;
    [Space(10)]


    //[Space(10)]
    [Header("Wheel References")]
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;
    [Space(10)]

    [Header("Camera References")]
    public GameObject camStillRotObject;
    public GameObject camLockRotObject;
    public GameObject camLookAtObject;
    private GameObject currentCamLock;
    public float camSmoothSpeed = 1.0f;
    private bool camFlipFlop;

    [Header("Boost Parameters")]
    public float boostMulitiplier = 1.25f;    
    public float maxBoostAmount;
    public float boostRechargeRate;
    public Slider boostUI;
    private float currentBoostAmount;
    private bool isBoosting = false;
    private float boostUpgradeVal = 1;

    private bool firstUpgrade;
    private float upgradeScale;
    private float wheelRadius = 0.38f;
    private Rigidbody rb;
    GameManager gameManager;
    

    [Header("Other")]
    public TimeRushTimer RushTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camera.main.transform.SetParent(null); //DETACHES CAMERA FROM PARENT (THE CAR)
        currentCamLock = camStillRotObject;
       
        

        /*boostUI.maxValue = maxBoostAmount;
        boostUI.value = 0;*/

        if (boostUI != null)
        {
            boostUI.maxValue = maxBoostAmount;
            boostUI.value = 0;
        }
    }
    void Update()
    {
        //print(rb.velocity.magnitude);
        if (Input.GetKeyDown(KeyCode.R))
        {
            //print("reset");
            //SceneManager.LoadScene(resetToScene.name);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("Level 1 Design");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (camFlipFlop == false)
            {
                currentCamLock = camLockRotObject;
            }
            else
            {
                currentCamLock = camStillRotObject;
            }
            camFlipFlop = !camFlipFlop;
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            if (camSmoothSpeed > 0.5f)
            {
                camSmoothSpeed -= 0.5f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            camSmoothSpeed += 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioManager.GlobalAudioManager.PlaySFX("Beep");
        }

        if (Input.GetKey(KeyCode.Space) && currentBoostAmount > 0)
        {
            ActivateBoost();
        }
        else
        {
            RechargeBoost();
        }

        UpdateBoostSlider();
    }

    void ActivateBoost()
    {
        isBoosting = true;
        currentBoostAmount -= Time.deltaTime;
        if (currentBoostAmount <= 0)
        {
            isBoosting = false;
        }
    }
    void RechargeBoost()
    {
        isBoosting = false;
        if (currentBoostAmount < maxBoostAmount)
        {
            currentBoostAmount += boostRechargeRate * Time.deltaTime; 
            currentBoostAmount = Mathf.Clamp(currentBoostAmount, 0, maxBoostAmount);
        }
    }
    void UpdateBoostSlider()
    {
        if (boostUI != null)
        {
            boostUI.value = currentBoostAmount;
        }
    }

    public void BoostUpgrade()
    {
        currentBoostAmount = maxBoostAmount;
    }


    void FixedUpdate()
    {
        HandleMovement();
        SpinWheels();
        CameraPosition();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            print("Damaged Enemy");
            if (other.GetComponent<Enemy>() != null)
            {
                DamageEnemy(other.GetComponent<Enemy>(), 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Building"))
        //{
        //    rb.AddForce()
        //}
    }

    void CameraPosition()
    {
        Vector3 desiredPosition = currentCamLock.transform.position;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, camSmoothSpeed * Time.deltaTime);
        Camera.main.transform.LookAt(camLookAtObject.transform);
    }

    

    void HandleMovement()
    {        
        float forwardInput = Input.GetAxis("Vertical"); 
        float turnInput = Input.GetAxis("Horizontal");
        float currentSpeed = maxSpeed * (isBoosting ? boostMulitiplier : 1f);

        if (forwardInput > 0)
        {
            rb.AddForce(transform.forward * forwardInput * accelerationForce, ForceMode.Acceleration);
        }
        else if (forwardInput < 0)
        {
            rb.AddForce(transform.forward * forwardInput * reverseForce, ForceMode.Acceleration);
            turnInput = 0 - turnInput;
        }
        
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentSpeed);
        
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

    public void DamageEnemy(Enemy enemyScript, float otherDamage)
    {
       
        Scene currentScene = SceneManager.GetActiveScene(); 
        float damage = rb.velocity.magnitude / maxSpeed * maxDamage;
        if (otherDamage > 0)
        {
            enemyScript.health -= otherDamage;
        }
        else
        {
            enemyScript.health -= damage;
        }
        
        if (enemyScript.health <= 0)
        {
            if (currentScene.name == "EndlessTest")
            {
                TestEndlessEnemyDeath(enemyScript);
               // StartCoroutine(EndlessEnemyDeath(enemyScript));
           
            }
        else if (currentScene.name == "TimeRushTest")
            {
                TestRushEnemyDeath(enemyScript);
                //StartCoroutine(TimeRushEnemyDeath(enemyScript));
            }
        else
            {
                Debug.Log("NULL SCENE");
            }
           
            
        }
    }

    /*IEnumerator EndlessEnemyDeath(Enemy enemyScript)
    {
        enemyScript.gameObject.SetActive(false);
        print("respawning");
        Upgrades.AddUpgradePoints();
        yield return new WaitForSeconds(enemyScript.respawnTime);
        enemyScript.gameObject.transform.position = enemyScript.spawnLoc;
        enemyScript.gameObject.SetActive(true);
        enemyScript.health = enemyScript.maxHealth;
    }

    IEnumerator TimeRushEnemyDeath(Enemy enemyScript)
    {
        enemyScript.gameObject.SetActive(false);
        print("respawning");
        Upgrades.AddUpgradePoints();
        RushTimer.AddTime(RushTimer.KillBonus);
        yield return new WaitForSeconds(enemyScript.respawnTime);
        enemyScript.gameObject.transform.position = enemyScript.spawnLoc;
        enemyScript.gameObject.SetActive(true);
        enemyScript.health = enemyScript.maxHealth;

    }*/


    void TestRushEnemyDeath(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        Upgrades.AddUpgradePoints();
        RushTimer.AddTime(RushTimer.KillBonus);

    }

    void TestEndlessEnemyDeath(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        Upgrades.AddUpgradePoints();
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
