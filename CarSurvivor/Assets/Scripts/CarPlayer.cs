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

public class CarPlayer : CarMechanics
{
    public Slider boostUI;

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
    // Start is called before the first frame update
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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        UpdateBoostSlider();

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

        if (Input.GetKey(KeyCode.LeftShift) && currentBoostAmount > 0)
        {
            ActivateBoost();
        }
        else
        {
            RechargeBoost();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.InitializeCar();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CameraPosition();
        CamBoom();
    }

    void UpdateBoostSlider()
    {
        if (boostUI != null)
        {
            boostUI.value = currentBoostAmount;
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


}
