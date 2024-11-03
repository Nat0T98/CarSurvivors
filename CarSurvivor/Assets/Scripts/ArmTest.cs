using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmTest : MonoBehaviour
{

    public GameObject point1;
    public GameObject point2;
    public GameObject legArm;
    public GameObject point3;
    public GameObject legArm2;
    private Vector3 armStartScale;

    public float armHeight;
    public float armBetween = 0.5f;

    void Start()
    {
        ArmInitialise(legArm);
    }

    
    void Update()
    {
        MoveBetween(point1, legArm, point2);
        LookAt(legArm, point2);
        ScaleArm(point1, legArm, point2);

        MoveBetween(point2, legArm2, point3);
        LookAt(legArm2, point3);
        ScaleArm(point2, legArm2, point3);
        MoveBetweenForJoints(point1, point2, point3, armBetween, armHeight);
    }

    void MoveBetween(GameObject armBase, GameObject arm, GameObject tip)
    {
        Vector3 halfPoint = (armBase.transform.position + tip.transform.position) / 2;
        arm.transform.position = halfPoint;
    }

    void LookAt(GameObject arm, GameObject tip)
    {
        arm.transform.LookAt(tip.transform);
    }

    void ScaleArm(GameObject armBase, GameObject arm, GameObject tip)
    {
        float armScale = Vector3.Distance(armBase.transform.position, tip.transform.position);
        arm.transform.localScale = new Vector3(armStartScale.x, armStartScale.y, armScale);
    }

    void ArmInitialise(GameObject arm)
    {
        armStartScale = arm.transform.localScale;
    }

    void MoveBetweenForJoints(GameObject arm1, GameObject armMiddle, GameObject arm2, float pointBetween, float pointY)
    {
        Vector3 halfPoint = Vector3.Lerp(arm1.transform.position, arm2.transform.position, pointBetween);
        halfPoint += new Vector3(0, pointY, 0);
        armMiddle.transform.position = halfPoint;
    }
}
