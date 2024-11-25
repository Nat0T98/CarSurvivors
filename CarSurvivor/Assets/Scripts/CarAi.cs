using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class CarAi : CarMechanics
{
    public NavMeshAgent agent;
    public Transform target;
    public float vert;
    public float horiz;

    public float lookAheadDistance = 5f;
    public float alignmentTolerance = 5f;
    public float cornerCoordsShift = 5f;
    public float dontDriftAngle = 15f;

    private bool shouldDrift;
    private Vector3 firstCorner;
    private Vector3 secondCorner;

    private Vector3 spawnPosAI;
    private quaternion spawnRotAI;

    public GameObject velotest;
    public GameObject cornerObj;


    //public float oversteerStrength = 2f;
    //public float cornerCoordsShift = 5f;
    //public float minCornerSpeed = 0.1f;
    //public float slowestAngleCorner = 90f;
    //public float slowDist = 15f;
    //public float slowLingerSpeed = 1f;
    //private float slowFactor;
    //private bool moveVeloDriftCheckOnce;


    protected override void Start()
    {
        base.Start();
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;

        if (target != null)
            agent.SetDestination(target.position);

        spawnPosAI = transform.position;
        spawnRotAI = transform.rotation;
        
    }

    protected override void Update()
    {
        base.Update();
        setCorners();
        forwardInput = vert;
        turnInput = horiz;

        OldUpdate();


        if (Input.GetKeyDown(KeyCode.H))
        {
            transform.position = spawnPosAI;
            transform.rotation = spawnRotAI;
            agent.Warp(spawnPosAI);
        }


        //velotest.transform.position = transform.position;
        //velotest.transform.rotation = Quaternion.LookRotation(firstCorner - transform.position, Vector3.up);
        //cornerObj.transform.position = firstCorner;

        //print("distance: " + Vector3.Distance(transform.position, firstCorner));
        //print("corners length: " + agent.path.corners.Length);
        //print(shouldDrift + "drift");
    }

    void OldUpdate()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }

        horiz = LeftRight();
        vert = forward();

        agent.nextPosition = transform.position;
    }

    void setCorners()
    {
        firstCorner = agent.path.corners[1];
        if (agent.path.corners.Length > 2)
        {
            secondCorner = agent.path.corners[2];
        }
    }
    
    private float forward()
    {
        Vector3 direction = agent.steeringTarget - transform.position;
        direction.y = 0;
        
        float i = Vector3.Dot(transform.forward, direction.normalized);
        if (i > 0)
        {
            return 1f;
        }
        else if (i < 0)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }

        //vert = Mathf.Clamp(Vector3.Dot(transform.forward, direction.normalized), -1f, 1f);
    }

    private float LeftRight()
    {

        

        if (shouldDrift)
        {
            if ((veloDirCornerMatch() || Vector3.Distance(transform.position, firstCorner) <= lookAheadDistance) && agent.path.corners.Length > 2)
            {
                return driftLeftOrRight();
            }
            else
            {
                shouldDrift = false;
                return 0f;
            }
            

        }
        else
        {
            if (agent.path.corners.Length > 2 && (Vector3.Distance(transform.position, firstCorner) <= lookAheadDistance
                || Vector3.Distance(transform.position, secondCorner) <= lookAheadDistance) && firstToSecondCorner() >= dontDriftAngle) shouldDrift = true;

            Vector3 direction = firstCorner - transform.position;
            direction.y = 0;
            Vector3 cross = Vector3.Cross(transform.forward, direction.normalized);
            print("crosss: " + cross.y);
            return Mathf.Clamp(cross.y * 2, -1f, 1f);
        }
    }

    float driftLeftOrRight()
    {
        float crossProd = crossForLeftRight();

        //print("CROSSPROD: " + crossProd);
        if (crossProd < 0)
        {
            //right
            return -1f;
        }
        else if (crossProd > 0)
        {
            //left
            return 1f;
        }
        else
        {
            return 0f;
        }
    }

    float crossForLeftRight()
    {
        Vector3 carPosition = transform.position;

        Vector3 dirToFirstPoint = (firstCorner - carPosition).normalized;
        Vector3 dirToSecondPoint = (secondCorner - carPosition).normalized;

        Vector3 crossProd = Vector3.Cross(dirToFirstPoint, dirToSecondPoint);

        return crossProd.y;
    }

    float firstToSecondCorner()
    {
        Vector3 carToCornerDir = firstCorner - transform.position;
        Vector3 cornerToCornerDir = secondCorner - firstCorner;
        return Vector3.Angle(carToCornerDir, cornerToCornerDir);
    }

    bool veloDirCornerMatch()
    {
        float alignmentAngle = Vector3.Angle(rb.velocity, firstCorner - transform.position);
        //print(alignmentAngle + "RAAAAAGGGGHHHHHHH");
        return alignmentAngle > alignmentTolerance;
    }

    //Vector3 TurnCoordsCentre()
    //{

    //    Vector3 carPosition = transform.position;

    //    Vector3 dirToFirstPoint = (firstCorner - carPosition).normalized;
    //    Vector3 dirToSecondPoint = (secondCorner - carPosition).normalized;

    //    Vector3 rightVectorOfFirst = Vector3.Cross(Vector3.up, dirToFirstPoint).normalized;

    //    Vector3 crossProd = Vector3.Cross(dirToFirstPoint, dirToSecondPoint);

    //    if (crossProd.y < 0)
    //    {
    //        //right
    //        return firstCorner + rightVectorOfFirst * cornerCoordsShift;
    //    }
    //    else if (crossProd.y > 0)
    //    {
    //        //left
    //        return firstCorner - rightVectorOfFirst * cornerCoordsShift;
    //    }
    //    else
    //    {
    //        return Vector3.zero;
    //    }
    //}

    //private float CornerSpeed()
    //{
    //    if (agent.path.corners.Length > 2)
    //    {
    //        Vector3 currentPos = transform.position;
    //        Vector3 firstCorner = agent.path.corners[1];
    //        Vector3 secondCorner = agent.path.corners[2];
    //        float distanceToCorner = Vector3.Distance(currentPos, firstCorner);

    //        Vector3 carToCornerDir = firstCorner - currentPos;
    //        Vector3 cornerToCornerDir = secondCorner - firstCorner;
    //        float cornerAngle = Vector3.Angle(carToCornerDir, cornerToCornerDir);

    //        //float slowFactor = Mathf.Lerp(1f, minCornerSpeed, cornerAngle / slowestAngleCorner);

    //        //print(CalculateDecelerationByDistance(rb.velocity.magnitude, 5f, 15f) + "SLOW");
    //        //float slowFactor = CalculateDecelerationByDistance(rb.velocity.magnitude, slowSpeed, slowDist);

    //        //testObject.transform.position = firstCorner;
    //        //print(rb.velocity.magnitude);
    //        print(Vector3.Distance(currentPos, firstCorner) + " DISTANCE");
    //        print(cornerAngle + " ANGLE");

    //        if (distanceToCorner <= slowDist)
    //        {

    //            slowFactor = Mathf.Lerp(1f, minCornerSpeed, cornerAngle / slowestAngleCorner);
    //        }
    //        else
    //        {
    //            slowFactor = slowFactor <= 1f ? slowFactor + Time.deltaTime * slowLingerSpeed : slowFactor;
    //            slowFactor = Mathf.Min(slowFactor, 1f);
    //        }
    //        print(slowFactor + " SLOW");
    //        return slowFactor;
    //    }
    //    else
    //    {
    //        return 0.4f;
    //    }
    //}

    //float CalculateDecelerationByDistance(float initialSpeed, float finalSpeed, float distance)
    //{
    //    return (Mathf.Pow(finalSpeed, 2) - Mathf.Pow(initialSpeed, 2)) / (2 * distance);
    //}



    //void NewUpdate()
    //{
    //    if (target != null)
    //        agent.SetDestination(target.position);

    //    Vector3 lookAheadTarget = CalculateLookAheadPoint();
    //    Vector3 direction = lookAheadTarget - transform.position;

    //    float horizontalInput = CalculateSteering(direction);


    //    vert = 1f;
    //    horiz = horizontalInput;
    //    agent.nextPosition = transform.position;
    //    print(horizontalInput + "IUHGIALUSRTHGLA");
    //}

    //Vector3 CalculateLookAheadPoint()
    //{
    //    Vector3 lookAheadTarget = agent.steeringTarget;

    //    for (int i = 1; i < agent.path.corners.Length; i++)
    //    {
    //        Vector3 corner = agent.path.corners[i];
    //        if (Vector3.Distance(transform.position, corner) >= lookAheadDistance)
    //        {
    //            lookAheadTarget = corner;
    //            break;
    //        }
    //    }

    //    return lookAheadTarget;
    //}

    //float CalculateSteering(Vector3 direction)
    //{
    //    Vector3 cross = Vector3.Cross(transform.forward, direction.normalized);
    //    float horizontalInput = Mathf.Clamp(cross.y, -1f, 1f);

    //    if (NeedsPostTurnOversteer())
    //    {
    //        horizontalInput *= oversteerStrength;
    //        //print("NOT ALIGNED");
    //    }
    //    else
    //    {
    //        horizontalInput = Mathf.Lerp(horizontalInput, 0, 0.1f);
    //    }

    //    return horizontalInput;
    //}

    //bool NeedsPostTurnOversteer()
    //{
    //    if (agent.path.corners.Length < 2)
    //        return false;

    //    Vector3 nextSegmentDirection = (agent.path.corners[1] + (TurnCoordsCentre() - agent.path.corners[1])) - transform.position;

    //    float alignmentAngle = Vector3.Angle(rb.velocity, nextSegmentDirection);

    //    return alignmentAngle > alignmentTolerance;
    //}




}
