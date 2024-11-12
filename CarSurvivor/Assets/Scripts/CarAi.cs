using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public float vert;
    public float horiz;


    public float minCornerSpeed = 0.1f;
    public float slowestAngleCorner = 90f;
    public float slowDist = 15f;
    public float slowLingerSpeed = 1f;
    private float slowFactor;

    public GameObject testObject;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;

        if (target != null)
            agent.SetDestination(target.position);
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }

        Vector3 direction = agent.steeringTarget - transform.position;
        direction.y = 0;

        vert = Mathf.Clamp(Vector3.Dot(transform.forward, direction.normalized), -1f, 1f);

        Vector3 cross = Vector3.Cross(transform.forward, direction.normalized);
        horiz = Mathf.Clamp(cross.y, -1f, 1f);

        //vert *= CornerSpeed();

        agent.nextPosition = transform.position;
    }

    

    private float CornerSpeed()
    {
        if (agent.path.corners.Length > 2)
        {
            Vector3 currentPos = transform.position;
            Vector3 firstCorner = agent.path.corners[1];
            Vector3 secondCorner = agent.path.corners[2];
            float distanceToCorner = Vector3.Distance(currentPos, firstCorner);

            Vector3 carToCornerDir = firstCorner - currentPos;
            Vector3 cornerToCornerDir = secondCorner - firstCorner;
            float cornerAngle = Vector3.Angle(carToCornerDir, cornerToCornerDir);

            //float slowFactor = Mathf.Lerp(1f, minCornerSpeed, cornerAngle / slowestAngleCorner);
            
            //print(CalculateDecelerationByDistance(rb.velocity.magnitude, 5f, 15f) + "SLOW");
            //float slowFactor = CalculateDecelerationByDistance(rb.velocity.magnitude, slowSpeed, slowDist);

            testObject.transform.position = firstCorner;
            //print(rb.velocity.magnitude);
            print(Vector3.Distance(currentPos, firstCorner) + " DISTANCE");
            print(cornerAngle + " ANGLE");

            if (distanceToCorner <= slowDist)
            {
                
                slowFactor = Mathf.Lerp(1f, minCornerSpeed, cornerAngle / slowestAngleCorner);
            }
            else
            {
                slowFactor = slowFactor <= 1f ? slowFactor + Time.deltaTime * slowLingerSpeed : slowFactor;
                slowFactor = Mathf.Min(slowFactor, 1f);
            }
            print(slowFactor + " SLOW");
            return slowFactor;
        }
        else
        {
            return 0.4f;
        }
        
    }
    float CalculateDecelerationByDistance(float initialSpeed, float finalSpeed, float distance)
    {
        return (Mathf.Pow(finalSpeed, 2) - Mathf.Pow(initialSpeed, 2)) / (2 * distance);
    }
}
