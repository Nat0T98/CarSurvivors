using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmTest : MonoBehaviour
{

    public GameObject point1;
    public GameObject point2;
    public GameObject legArm;
    private Vector3 armStartScale;

    void Start()
    {
        ArmInitialise(legArm);
    }

    
    void Update()
    {
        MoveBetween(point1, legArm, point2);
        LookAt(legArm, point2);
        ScaleArm(point1, legArm, point2);
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
}
