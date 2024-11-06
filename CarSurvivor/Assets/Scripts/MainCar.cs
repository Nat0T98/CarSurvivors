using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
    public float steeringSpeed = 0.1f;
    public float maxDamage = 120f;
    public float maxHealth = 100f;
    public float health;  
    public Slider healthUI;
    [Space(10)]

    [Header("Upgrade  References")]
    public UpgradeManager Upgrades;
    public GameObject wheelSpinnerL;
    public GameObject wheelSpinnerR;
    public GameObject rammer;
    [Space(10)]

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
    [Space(10)]

    [Header("Turret")]
    public GameObject turretMain;
    private List<GameObject> targetedObjects = new List<GameObject>();
    public float turretRotSpeed = 1;
    public bool turretActive;
    public float turretFireRateUpgrade = 0.05f;
    public float turretFireRateLimit = 0.02f;
    [Space(10)]
    [Header("Wheel References")]
    public float skidThreshholdAngle;
    public GameObject smokeObj;
    public float smokeSpawnDelay = 0.5f;
    public float smokeSpawnVarience = 0.2f;
    private bool skidOnce = true;
    private bool canSmoke;

    [Serializable]
    public struct Wheels
    {
        public GameObject wheelObj;
        public GameObject wheelEffectObj;
    }
    public List<Wheels> wheels;
    [Space(10)]

    [Header("Camera References")]
    public GameObject camStillRotObject;
    public GameObject camLockRotObject;
    public GameObject camLookAtObject;
    private GameObject currentCamLock;
    public float camSmoothSpeed = 1.0f;
    private bool camFlipFlop;
    [Space(10)]

    [Header("Other")]
    public TimeRushTimer RushTimer;
    public Vector3 rotationPointOffset = new Vector3(0, 0, 2f);

    public GameObject gameOverScreen;
    private Vector3 spawnPos;

    private float steeringObj;

    public float boomExtraLength = 0;
    private float boomInitDist;
    private bool boomOnce;
    private bool isDrifting; 

    private void Awake()
    {
        GameManager.Instance.InitializeCar();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camera.main.transform.SetParent(null); //DETACHES CAMERA FROM PARENT (THE CAR)
        currentCamLock = camStillRotObject;

        spawnPos = transform.position;

        health = maxHealth;


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
        //print("Speed:" + rb.velocity.magnitude);
        
        
        //print(rb.velocity.magnitude);
        if (Input.GetKeyDown(KeyCode.R))
        {
            //print("reset");
            //SceneManager.LoadScene(resetToScene.name);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            transform.position = spawnPos;
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
            SFX_Manager.GlobalSFXManager.PlaySFX("Beep");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && currentBoostAmount > 0)
        {
            ActivateBoost();
        }
        else
        {
            RechargeBoost();
        }

        UpdateBoostSlider();

        if (turretActive)
        {
            Turret();
        }
        

        if (healthUI != null)
        {
            healthUI.maxValue = maxHealth;
            healthUI.value = health;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            health += 99999999f;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Upgrades.EnemyPointWorth = 999999;
            Upgrades.AddUpgradePoints();
        }

        wheelSkidMarks();
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
        CamBoom();
    }

    public void ramTriggerEnter(Collider otherCol)
    {
        if (otherCol.CompareTag("Enemy"))
        {
            print("Damaged Enemy");
            if (otherCol.GetComponent<Enemy>() != null)
            {
                DamageEnemy(otherCol.GetComponent<Enemy>(), 0);
            }
        }
    }

    public void areaTriggerEnter(Collider otherCol)
    {
        if (otherCol.CompareTag("Enemy"))
        {
            targetedObjects.Add(otherCol.gameObject);
        }
    }
    public void areaTriggerExit(Collider otherCol)
    {
        if (otherCol.CompareTag("Enemy"))
        {
            targetedObjects.Remove(otherCol.gameObject);
            //print(otherCol.gameObject.name);
        }
    }

    void CameraPosition()
    {
        Vector3 desiredPosition = currentCamLock.transform.position;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, camSmoothSpeed * Time.deltaTime);
        Camera.main.transform.LookAt(camLookAtObject.transform);
    }

    void CamBoom()
    {
        Vector3 endPoint = currentCamLock.transform.position;
        Vector3 startPoint = new Vector3(transform.position.x, currentCamLock.transform.position.y, transform.position.z);

        Vector3 direction = (endPoint - startPoint).normalized;
        if (boomOnce == false)
        {
            boomInitDist = Vector3.Distance(startPoint, endPoint) + boomExtraLength;
            boomOnce = true;
        }
        

        if (Physics.Raycast(startPoint, direction, out RaycastHit hitInfo, boomInitDist))
        {
            currentCamLock.transform.position = hitInfo.point;
        }

        
        Debug.DrawLine(startPoint, startPoint + direction * boomInitDist, Color.blue);
    }
    

    void HandleMovement()
    {        
        float forwardInput = Input.GetAxis("Vertical"); 
        float turnInput = Input.GetAxis("Horizontal");
        float currentSpeed = maxSpeed * (isBoosting ? boostMulitiplier : 1f);
        //float turnAmount = (isDrifting ? driftTurnSpeed : turnSpeed) * turnInput * Time.fixedDeltaTime;

        //forward and backward movement
        if (forwardInput > 0)
        {
            rb.AddForce(transform.forward * forwardInput * accelerationForce, ForceMode.Acceleration);
        }
        else if (forwardInput < 0)
        {
            rb.AddForce(transform.forward * forwardInput * reverseForce, ForceMode.Acceleration);
            //turnInput = 0 - turnInput;
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentSpeed);

        //turn movement
        //if (Input.GetKey(KeyCode.A))
        //{
        //    steeringObj = Mathf.MoveTowards(steeringObj, -1, steeringSpeed * Time.deltaTime);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    steeringObj = Mathf.MoveTowards(steeringObj, 1, steeringSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    steeringObj = Mathf.MoveTowards(steeringObj, 0, steeringSpeed * Time.deltaTime);
        //}

        steeringObj = Mathf.MoveTowards(steeringObj, turnInput, steeringSpeed * Time.deltaTime);

        steeringObj = Mathf.Clamp(steeringObj, -1, 1);

        if (rb.velocity.magnitude > 0.1f)
        {
            float turn = steeringObj * ((rb.velocity.magnitude / maxSpeed) * turnSpeed) * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            Vector3 rotationPointWorld = transform.TransformPoint(rotationPointOffset);
            //Quaternion rotation = Quaternion.Euler(0f, turn, 0f);

            rb.MoveRotation(rb.rotation * turnRotation);

            Vector3 directionToCar = transform.position - rotationPointWorld;
            Vector3 newCarPosition = turnRotation * directionToCar + rotationPointWorld;
            rb.MovePosition(newCarPosition);
        }

        //if (rb.velocity.magnitude < maxSpeed)
        //{
        //    if (isDrifting)
        //    {
        //        rb.AddForce(transform.forward * forwardInput * driftForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        //    }
        //    else
        //    {
        //        rb.AddForce(transform.forward * forwardInput * accelerationForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        //    }
        //}

        //if (isDrifting)
        //{
        //    rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * rb.velocity.magnitude, driftFactor);
        //}



        //if (rb.velocity.magnitude > 0.1f)
        //{
        //    rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turnAmount, 0));
        //}
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

        foreach (var wheel in wheels)
        {
            RotateWheel(wheel.wheelObj, rotationAngle);
        }
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
            enemyScript.health = 0;
            if (currentScene.name == "Showcase")
            {
                TestEndlessEnemyDeath(enemyScript);
                // StartCoroutine(EndlessEnemyDeath(enemyScript));

            }
       /* else if (currentScene.name == "EndlessTest")
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
            }*/
           
            
        }
    }
    #region Old Enumerators
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
    #endregion

    


    /*void TestRushEnemyDeath(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        Upgrades.AddUpgradePoints();
        RushTimer.AddTime(RushTimer.KillBonus);

    }*/

    void TestEndlessEnemyDeath(Enemy enemy)
    {
        Upgrades.AddUpgradePoints();
        Destroy(enemy.gameObject);
        
    }


    void Turret()
    {
        float closestDistance = 9999999f;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in targetedObjects)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 targDir = nearestEnemy.transform.position - turretMain.transform.position;
            Quaternion targRot = Quaternion.LookRotation(targDir);
            turretMain.transform.rotation = Quaternion.Slerp(turretMain.transform.rotation, targRot, turretRotSpeed * Time.deltaTime);
                //LookAt(nearestEnemy.transform.position);
            GetComponent<Firing>().TurretFire();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameOverScreen != null)
            {
                gameOverScreen.GetComponent<GameOver>().GameOverScreen();
            }
        }
    }

    public void CarMiniGunUpgrade()
    {
        turretMain.SetActive(true);
        turretActive = true;
        GetComponent<Firing>().fireRate -= turretFireRateUpgrade;
        if (GetComponent<Firing>().fireRate < turretFireRateLimit)
        {
            GetComponent<Firing>().fireRate = turretFireRateLimit;
        }
    }

    private void wheelSkidMarks()
    {
        if (Vector3.Angle(transform.forward.normalized, rb.velocity.normalized) >= skidThreshholdAngle && rb.velocity.magnitude > 10)
        {
            canSmoke = true;
            foreach (var wheel in wheels)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                if (skidOnce)
                {
                    StartCoroutine(SpawnSmoke(wheel.wheelObj));
                }
            }
            skidOnce = false;
        }
        else
        {
            skidOnce = true;
            canSmoke = false;
            foreach (var wheel in wheels)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    IEnumerator SpawnSmoke(GameObject wheelobj)
    {
        while(canSmoke)
        {
            Instantiate(smokeObj, wheelobj.transform.position + new Vector3(0, 0.3f, 0), wheelobj.transform.rotation);
            yield return new WaitForSeconds(smokeSpawnDelay + UnityEngine.Random.Range(-smokeSpawnVarience, smokeSpawnVarience));
        }
    }

    public void ScreenShake(Vector3 shakeOrigin, float power)
    {

    }
}
