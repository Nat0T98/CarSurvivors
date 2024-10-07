using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCar : MonoBehaviour
{
    public float accelerationForce = 50f; 
    public float reverseForce = 30f; 
    public float maxSpeed = 20f; 
    public float turnSpeed = 100f;
    public GameObject wheelSpinner;

    private bool firstUpgrade;
    private float upgradeScale;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
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
        }

        
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        
        if (rb.velocity.magnitude > 0.1f) //could interpolate turning speed based off car speed. e.g: turn speed = 0.1 if car speed = 10, turn speed = 1 if car speed = 100
        {
            float turn = turnInput * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    public void MelleUpgradeTest()
    {
        print("COLLECTED");
        if (firstUpgrade == false)
        {
            wheelSpinner.SetActive(true);
            firstUpgrade = true;
            upgradeScale = wheelSpinner.transform.localScale.x;
        }
        else
        {
            upgradeScale += 0.01f;
            wheelSpinner.transform.localScale = new Vector3(upgradeScale, wheelSpinner.transform.localScale.y, wheelSpinner.transform.localScale.z);
            print(upgradeScale);
        }
    }
}
