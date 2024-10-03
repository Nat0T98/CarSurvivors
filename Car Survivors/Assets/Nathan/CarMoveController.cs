using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoveController : MonoBehaviour
{
    [Header("Wheel References")]
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backLeft;
    [SerializeField] WheelCollider backRight;
    [Space(30)]

    [Header("Car Settings")]
    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 15f;


    private float currAccelleration = 0f;
    private float currBrakingForce = 0f;
    private float currTurnAngle = 0f;




    private void FixedUpdate()
    {

        currAccelleration = acceleration * Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            currBrakingForce = brakingForce;
        else 
            currBrakingForce = 0f;


        frontRight.motorTorque = currAccelleration;
        frontLeft.motorTorque = currAccelleration;

        frontRight.brakeTorque = currBrakingForce;
        frontLeft.brakeTorque = currBrakingForce;
        backRight.brakeTorque = currBrakingForce;
        backLeft.brakeTorque = currBrakingForce;

        currTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currTurnAngle;
        frontRight.steerAngle = currTurnAngle;

    }
}
