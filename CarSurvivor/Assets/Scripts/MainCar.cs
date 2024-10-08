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

    private Rigidbody rb;

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
            print("those who know");
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
        // Calculate how much the wheels should rotate based on car's velocity
        float distanceTraveled = rb.velocity.magnitude * Time.deltaTime;
        float rotationAngle = (distanceTraveled / (2 * Mathf.PI * wheelRadius)) * 360f;

        // Rotate the wheels around their local X-axis (forward axis)
        RotateWheel(wheel1, rotationAngle);
        RotateWheel(wheel2, rotationAngle);
        RotateWheel(wheel3, rotationAngle);
        RotateWheel(wheel4, rotationAngle);
    }

    void RotateWheel(GameObject wheel, float rotationAngle)
    {
        // Apply the rotation to the wheel
        wheel.transform.Rotate(Vector3.right, rotationAngle, Space.Self);
    }


}
