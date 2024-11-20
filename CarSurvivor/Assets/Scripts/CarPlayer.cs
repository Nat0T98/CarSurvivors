using UnityEngine;

public class CarPlayer : CarMechanics
{
    [Header("Camera References")]
    public GameObject camStillRotObject;
    public GameObject camLockRotObject;
    public GameObject camLookAtObject;
    private GameObject currentCamLock;
    public float camSmoothSpeed = 1.0f;
    private bool camFlipFlop;
    public float boomExtraLength = 0;
    private float boomInitDist;
    private bool boomOnce;

    private GameManager gameManager;
    

    protected override void Start()
    {
        base.Start();
        currentCamLock = camStillRotObject;

        if (boostUI != null)
        {
            boostUI.maxValue = maxBoostAmount;
            boostUI.value = 0;
        }
    }

    
    protected override void Update()
    {
        base.Update();
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        UpdateBoostSlider();

        if (Input.GetKeyDown(KeyCode.R))
        {
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateBoost();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DeactivateBoost();
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
    }

    protected override void Awake()
    {
        base.Awake();
        //GameManager.Instance.InitializeCar();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CameraPosition();
        CamBoom();
    }

    void ActivateBoost()
    {
        if (currentBoostAmount > 0)
        {
            SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", false);
            drivingPitch = 1.4f;//Higher SFX pitch whilst boosting
            SFX_Manager.GlobalSFXManager.PlayDrivingSFX("Driving", true, drivingPitch, 0.2f);
            isBoosting = true;
            SFX_Manager.GlobalSFXManager.PlayBoostSFX(0.7f);
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
        currentBoostAmount = maxBoostAmount;
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


}
