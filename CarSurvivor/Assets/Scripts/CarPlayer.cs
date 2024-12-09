using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CarPlayer : CarMechanics
{
    [Header("Camera References")]
    public GameObject mainRig;
    public GameObject camStillRotObject;
    public GameObject camLockRotObject;
    public GameObject camLookAtObject;
    private GameObject currentCamLock;
    public float camSmoothSpeed = 1.0f;
    public bool smoothRot;
    public float camRotSmoothSpeed = 1.0f;
    private bool camFlipFlop;
    public float boomExtraLength = 0;
    private float boomInitDist;
    private bool boomOnce;

    public Vector3 initialCam;
    public Vector3 initialRot;
    public bool input = true;
    public bool doIntro;
    public float introiAttachCamTime = 2f;
    public float introTimeAfterCam = 3f;
    public bool camMove;

    [Header("Camera Shake References")]
    public float shakeDuration = 0.5f; 
    public float shakeStrength = 1.0f; 
    public float shakeDampingSpeed = 0.2f;

    private float shakeTime = 0f;

    public GameObject hudShakeObj;

    //public float camFollowSpeed = 5f;
    //public float camInertia = 1.5f;
    //public float camReturnSpeed = 3f;
    //private Vector3 camVelocity;

    private GameManager gameManager;
    private bool shouldFreezeTime;

    protected override void Start()
    {
        base.Start();
        if (GetComponent<Camera>() != null)
        {
            mainRig.transform.SetParent(null); //DETACHES CAMERA FROM PARENT (THE CAR)
            print("seygkwygfksuiyghkjsyhghksaghkg");
        }
        mainRig.transform.SetParent(null); //DETACHES CAMERA FROM PARENT (THE CAR)
        currentCamLock = camStillRotObject;

        if (boostUI != null)
        {
            boostUI.maxValue = maxBoostAmount;
            boostUI.value = 0;
        }

        if (doIntro)
        {
            StartCoroutine(Intro());
        }

    }

    
    protected override void Update()
    {
        base.Update();
        if (input)
        {
            forwardInput = Input.GetAxis("Vertical");
            turnInput = Input.GetAxis("Horizontal");
        }
        

        UpdateBoostSlider();
        CamShake();
        UiCanvasShake();

        if (Input.GetKeyDown(KeyCode.K))
        {
            health += 99999999f;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Upgrades.EnemyPointWorth = 999999;
            Upgrades.AddUpgradePoints();
        }

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    TriggerShake(0.2f, 0.2f);
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
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

        /*if (Input.GetKeyDown(KeyCode.Period))
        {
            camSmoothSpeed += 0.5f;
        }*/

        if (Input.GetKeyDown(KeyCode.B))
        {
            SFX_Manager.GlobalSFXManager.PlaySFX("Beep");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateBoost();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DeactivateBoost();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            VehicleChange(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            VehicleChange(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            VehicleChange(3);
        }


        if (isBoosting)
        {
            currentBoostAmount -= boostUsageRate * Time.deltaTime;
            if (currentBoostAmount <= 0)
            {
                currentBoostAmount = 0;
                DeactivateBoost();
            }
        }
        else
        {
            RechargeBoost();
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    if (shouldFreezeTime == false)
        //    {
        //        Time.timeScale = 0.3f;
        //    }
        //    else
        //    {
        //        Time.timeScale = 1f;
        //    }
        //    shouldFreezeTime = !shouldFreezeTime;
        //}

        //if (camMove)
        //{
        //    CameraPosition();
        //}

        //CamBoom();
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerShake(0.2f, 0.2f);
    }

    protected override void Awake()
    {
        base.Awake();
        //GameManager.Instance.InitializeCar();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (camMove)
        {
            CameraPosition();
        }

        CamBoom();
    }

    void ActivateBoost()
    {
        if (currentBoostAmount > 0)
        {
            SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", false);
            drivingPitch = 1.4f;//Higher SFX pitch whilst boosting
            SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", true, drivingPitch, 0.3f);
            isBoosting = true;
            SFX_Manager.GlobalSFXManager.PlayBoostSFX(0.6f);
        }
    }

    void DeactivateBoost()
    {
        SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", false);
        drivingPitch = 1f;
        isBoosting = false;
        SFX_Manager.GlobalSFXManager.StopBoostSFX();
    }

    void RechargeBoost()
    {
        if (!isBoosting && currentBoostAmount < maxBoostAmount)
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
        maxBoostAmount += boostUpgradeAdd;
        boostUI.maxValue = maxBoostAmount;
    }

    void CameraPosition()
    {
        Vector3 desiredPosition = currentCamLock.transform.position;
        mainRig.transform.position = Vector3.Lerp(mainRig.transform.position, desiredPosition, camSmoothSpeed * Time.deltaTime);
        
        if (smoothRot)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camLookAtObject.transform.position - mainRig.transform.position);
            mainRig.transform.rotation = Quaternion.Slerp(mainRig.transform.rotation, targetRotation, camRotSmoothSpeed * Time.deltaTime);
        }
        else
        {
            mainRig.transform.LookAt(camLookAtObject.transform);
        }





        //if (rb.velocity.magnitude > 0.1f) 
        //{
        //    camVelocity += rb.velocity * camInertia * Time.deltaTime;
        //}

        //camVelocity = Vector3.Lerp(camVelocity, Vector3.zero, Time.deltaTime * camReturnSpeed);

        ////Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, currentCamLock.transform.position + camVelocity, Time.deltaTime * camFollowSpeed);

        //Camera.main.transform.position = currentCamLock.transform.position - camVelocity;

    }

    void CamBoom()
    {
        Vector3 endPoint = currentCamLock.transform.position;
        Vector3 startPoint = new Vector3(transform.position.x, currentCamLock.transform.position.y, transform.position.z);

        Vector3 direction = (endPoint - startPoint).normalized;
        if (boomOnce == false)
        {
            boomInitDist = Vector3.Distance(startPoint, endPoint);
            boomOnce = true;
        }

        float boomDist = boomInitDist + boomExtraLength;

        if (Physics.Raycast(startPoint, direction, out RaycastHit hitInfo, boomDist))
        {
            currentCamLock.transform.position = hitInfo.point;
        }
        else
        {
            currentCamLock.transform.position = endPoint;
        }


        Debug.DrawLine(startPoint, startPoint + direction * boomInitDist, Color.blue);
    }

    IEnumerator Intro()
    {
        input = false;
        forwardInput = 1f;
        
        yield return new WaitForSeconds(introTimeAfterCam);
        input = true;
    }

    void CamShake()
    {
        if (shakeTime > 0)
        {
            Vector3 shakeOffset = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                0f
            ) * shakeStrength;

            Camera.main.transform.localPosition = Vector3.zero + shakeOffset;

            shakeTime -= Time.deltaTime * shakeDampingSpeed;

            if (shakeTime <= 0)
            {
                shakeTime = 0f;
                Camera.main.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void TriggerShake(float duration, float strength)
    {
        shakeDuration = duration;
        shakeStrength = strength;
        shakeTime = shakeDuration;
    }

    void UiCanvasShake()
    {
        hudShakeObj.transform.localPosition = new Vector3(smoothedLocalAccel.x * 2 * -1, smoothedLocalAccel.z * 1 * -1, 0);
    }
}
