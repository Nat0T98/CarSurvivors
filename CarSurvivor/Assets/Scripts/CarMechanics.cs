using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarMechanics : MonoBehaviour
{
    // DAN WAS HERE
    bool ISDRIFTING = false;
   
    public float forwardInput;
    public float turnInput;

    [Header("Car Stats")]
    public float accelerationForce = 50f;
    public float reverseForce = 30f;
    public float maxSpeed = 20f;
    public float turnSpeed = 150f;
    public float turnSlowDownAtPercent = 0.3f;
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
    public float boostMaxSpeedMulitiplier = 1.25f;
    public float boostAccelerationMult = 1.5f;
    public float boostDecelSpeed = 1f;
    public float maxBoostAmount;
    public float boostRechargeRate;
    public Slider boostUI;
    protected float currentBoostAmount;
    protected bool isBoosting = false;
    protected bool isBoostSFX = false;
    protected float drivingPitch = 1f;
    //private float boostUpgradeVal = 1;
    protected float boostUsageRate = 1f;
    private float boostToNormPercent;

    private bool firstUpgrade;
    private float upgradeScale;
    private float wheelRadius = 0.38f;
    
    //public GameManager gameManager;
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
        public Vector3 car1WheelPos;
        public Vector3 car2WheelPos;
        public Vector3 car3WheelPos;
    }
    public List<Wheels> wheels;
    [Space(10)]

    //[Header("Camera References")]
    //public GameObject camStillRotObject;
    //public GameObject camLockRotObject;
    //public GameObject camLookAtObject;
    //private GameObject currentCamLock;
    //public float camSmoothSpeed = 1.0f;
    //private bool camFlipFlop;
    [Space(10)]

    [Header("Lean Settings")]
    public float maxLean = 10;
    public float smoothingFactor = 1.0f;
    public float xLeanStrength = 1f;
    public float zLeanStrength = 1f;
    private Vector3 previousVelocity;
    private Vector3 localAccel;
    private Vector3 smoothedLocalAccel;


    [Space(10)]
    [Header("Other")]
    public Rigidbody rb;
    public Vector3 rotationPointOffset = new Vector3(0, 0, 2f);

    public GameObject gameOverScreen;
    public Vector3 spawnPos;

    private float steeringObj;
    public float slowTurnMultiplier = 1.0f;

    //public float boomExtraLength = 0;
    //private float boomInitDist;
    //private bool boomOnce;
    //private bool isDrifting;
    private CarAi carAiScript;
    //public bool useAi = true;
    public TimeRushTimer timeRush;

    public GameObject car1Body;
    public GameObject car2Body;
    public GameObject car3Body;
    private GameObject currentBody;

    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    protected virtual void Awake()
    {
       //GameManager.Instance.InitializeCar();
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentBody = car1Body;
        //currentCamLock = camStillRotObject;

        spawnPos = transform.position;

        health = maxHealth;

        carAiScript = GetComponent<CarAi>();


        /*boostUI.maxValue = maxBoostAmount;
        boostUI.value = 0;*/
        previousVelocity = rb.velocity;
        smoothedLocalAccel = Vector3.zero;


    }
    protected virtual void Update()
    {
        #region Old Update Code
 //print("Speed:" + rb.velocity.magnitude);


        //print(rb.velocity.magnitude);
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    //print("reset");
        //    //SceneManager.LoadScene(resetToScene.name);
        //    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //    transform.position = spawnPos;
        //}



        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if (camFlipFlop == false)
        //    {
        //        currentCamLock = camLockRotObject;
        //    }
        //    else
        //    {
        //        currentCamLock = camStillRotObject;
        //    }
        //    camFlipFlop = !camFlipFlop;
        //}

        //if (Input.GetKeyDown(KeyCode.Comma))
        //{
        //    if (camSmoothSpeed > 0.5f)
        //    {
        //        camSmoothSpeed -= 0.5f;
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Period))
        //{
        //    camSmoothSpeed += 0.5f;
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    SFX_Manager.GlobalSFXManager.PlaySFX("Beep");
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    isDrifting = true;
        //}
        //else
        //{
        //    isDrifting = false;
        //}

        //if (Input.GetKey(KeyCode.LeftShift) && currentBoostAmount > 0)
        //{
        //    ActivateBoost();
        //}
        //else
        //{
        //    RechargeBoost();
        //}

        #endregion
       


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

    


    protected virtual void FixedUpdate()
    {
        HandleMovement();
        SpinWheels();
        //CameraPosition();
        //CamBoom();
        CarLean();
    }

    public void ramTriggerEnter(Collider otherCol)
    {
        if (otherCol.CompareTag("Enemy"))
        {
           
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
    #region CameraPosition()
//void CameraPosition()
    //{
    //    Vector3 desiredPosition = currentCamLock.transform.position;
    //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, camSmoothSpeed * Time.deltaTime);
    //    Camera.main.transform.LookAt(camLookAtObject.transform);
    //}

    //void CamBoom()
    //{
    //    Vector3 endPoint = currentCamLock.transform.position;
    //    Vector3 startPoint = new Vector3(transform.position.x, currentCamLock.transform.position.y, transform.position.z);

    //    Vector3 direction = (endPoint - startPoint).normalized;
    //    if (boomOnce == false)
    //    {
    //        boomInitDist = Vector3.Distance(startPoint, endPoint) + boomExtraLength;
    //        boomOnce = true;
    //    }


    //    if (Physics.Raycast(startPoint, direction, out RaycastHit hitInfo, boomInitDist))
    //    {
    //        currentCamLock.transform.position = hitInfo.point;
    //    }


    //    Debug.DrawLine(startPoint, startPoint + direction * boomInitDist, Color.blue);
    //}
    #endregion
    


    void HandleMovement()
    {
        //float forwardInput;
        //float turnInput;
        float maxBoostSpeed;
        //pitch based on speed of car,
        /*float speedPercentage = rb.velocity.magnitude / maxSpeed;
        float pitch = Mathf.Clamp(speedPercentage + 0.5f, 0.5f, 2f);*/

        //if (useAi)
        //{
        //    forwardInput = carAiScript.vert;
        //    turnInput = carAiScript.horiz;
        //}
        //else
        //{
        //    forwardInput = Input.GetAxis("Vertical");
        //    turnInput = Input.GetAxis("Horizontal");
        //}


        if (isBoosting)
        {
            maxBoostSpeed = maxSpeed * boostMaxSpeedMulitiplier;
            boostToNormPercent = 0f;
        }
        else
        {
            boostToNormPercent = Mathf.Clamp(boostToNormPercent + Time.deltaTime * boostDecelSpeed, 0, 1);
            maxBoostSpeed = Mathf.Lerp(maxSpeed * boostMaxSpeedMulitiplier, maxSpeed, boostToNormPercent);
        }

        //float turnAmount = (isDrifting ? driftTurnSpeed : turnSpeed) * turnInput * Time.fixedDeltaTime;

        //forward and backward movement
        if (forwardInput > 0)
        {
            rb.AddForce(transform.forward * forwardInput * accelerationForce * (isBoosting ? boostAccelerationMult : 1f), ForceMode.Acceleration);
            if(CompareTag("Player"))
            {
                SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", true, drivingPitch, 0.3f);
            }
            

        }
        else if (forwardInput < 0)
        {
            drivingPitch = 0.8f;
            rb.AddForce(transform.forward * forwardInput * reverseForce, ForceMode.Acceleration);
            //turnInput = 0 - turnInput;
            if (CompareTag("Player"))
            {
                SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", true, drivingPitch, 0.3f);
            }
            
        }
        else
        {
            // Stop driving sound when stationary
            SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", false);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxBoostSpeed);

       
        steeringObj = Mathf.MoveTowards(steeringObj, turnInput, steeringSpeed * Time.deltaTime);

        steeringObj = Mathf.Clamp(steeringObj, -1, 1);

        if (rb.velocity.magnitude > 0.1f)
        {
            //float turn = steeringObj * (rb.velocity.magnitude / maxSpeed * turnSpeed) * Time.deltaTime;
            float turn = steeringObj * (Mathf.Clamp01(rb.velocity.magnitude / maxSpeed / turnSlowDownAtPercent) * turnSpeed) * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            Vector3 rotationPointWorld = transform.TransformPoint(rotationPointOffset);
            //Quaternion rotation = Quaternion.Euler(0f, turn, 0f);

            rb.MoveRotation(rb.rotation * turnRotation);

            Vector3 directionToCar = transform.position - rotationPointWorld;
            Vector3 newCarPosition = turnRotation * directionToCar + rotationPointWorld;
            rb.MovePosition(newCarPosition);
        }
        #region Old Code 
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
        #endregion
       
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
            //TestEndlessEnemyDeath(enemyScript);
            switch (currentScene.name)
            {
                case "Showcase":
                    TestEndlessEnemyDeath(enemyScript);
                    break;
                case "Time Rush":
                    TestRushEnemyDeath(enemyScript);
                    break;
            }
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




    void TestRushEnemyDeath(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        Upgrades.AddUpgradePoints();
        timeRush.AddTime();

    }

    void TestEndlessEnemyDeath(Enemy enemy)
    {
        Upgrades.AddUpgradePoints();
        enemy.gameObject.SetActive(false);
    }


    void Turret()
    {
        float closestDistance = 9999999f;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in targetedObjects)
        {
            if (enemy != null && enemy.activeSelf)
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
                SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", false);
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
        Vector3 horizontalVeloctiy = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z);
        ISDRIFTING = Vector3.Angle(horizontalForward, horizontalVeloctiy) >= skidThreshholdAngle && horizontalVeloctiy.magnitude > 5;
       
        if (ISDRIFTING && IsGrounded())
        {
            //Debug.Log("STARTED");
            canSmoke = true;
            if (CompareTag("Player"))
            {
                SFX_Manager.GlobalSFXManager.PlayDriftFX(0.3f);
            }
           
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
            if (CompareTag("Player"))
            {
                SFX_Manager.GlobalSFXManager.StopDriftSFX();
            }
            foreach (var wheel in wheels)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    IEnumerator SpawnSmoke(GameObject wheelobj)
    {
        while (canSmoke)
        {
            Instantiate(smokeObj, wheelobj.transform.position + new Vector3(0, 0.3f, 0), wheelobj.transform.rotation);
            yield return new WaitForSeconds(smokeSpawnDelay + UnityEngine.Random.Range(-smokeSpawnVarience, smokeSpawnVarience));
        }
    }

    public void VehicleChange(int vehicle)
    {
        if (vehicle == 1)
        {
            currentBody.SetActive(false);
            car1Body.SetActive(true);
            currentBody = car1Body;
            foreach (var wheel in wheels)
            {
                wheel.wheelObj.transform.localPosition = wheel.car1WheelPos;
            }
        }
        else if (vehicle == 2)
        {
            currentBody.SetActive(false);
            car2Body.SetActive(true);
            currentBody = car2Body;
            foreach (var wheel in wheels)
            {
                wheel.wheelObj.transform.localPosition = wheel.car2WheelPos;
            }
        }
        else if (vehicle == 3)
        {
            currentBody.SetActive(false);
            car3Body.SetActive(true);
            currentBody = car3Body;
            foreach (var wheel in wheels)
            {
                wheel.wheelObj.transform.localPosition = wheel.car3WheelPos;
            }
        }
    }

    public void ScreenShake(Vector3 shakeOrigin, float power)
    {

    }

    private void CarLean()
    {
        Vector3 worldAcceleration = (rb.velocity - previousVelocity) / Time.fixedDeltaTime;
        localAccel = transform.InverseTransformDirection(worldAcceleration);
        smoothedLocalAccel = Vector3.Lerp(smoothedLocalAccel, localAccel, smoothingFactor);
        previousVelocity = rb.velocity;
        float yVal = currentBody == car2Body ? -1f : 1f;
        float xLean = Mathf.Clamp(smoothedLocalAccel.x, -maxLean, maxLean);
        float zLean = Mathf.Clamp(smoothedLocalAccel.z, -maxLean, maxLean);
        currentBody.transform.localRotation = Quaternion.Euler(xLean * xLeanStrength * yVal, -90f * yVal, zLean * zLeanStrength * yVal);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(rb.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
