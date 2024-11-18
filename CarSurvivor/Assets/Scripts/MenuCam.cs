using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MenuCam : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    public float moveTime = 3f;
    public Vector3 menuPos;
    public Quaternion menuRot;
    public Vector3 endPos;
    public Quaternion endRot;

    private float time1;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation; //.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        StartMove();
    }

    void StartMove()
    {
        time1 += Time.deltaTime;
        float t = Mathf.Clamp01(time1 / moveTime);
        float a = Mathf.SmoothStep(0, 1 , t);
        transform.position = Vector3.Slerp(startPos, menuPos, a);
        transform.rotation = Quaternion.Slerp(startRot, menuRot, a);
        //transform.rotation = Quaternion.RotateTowards(startRot, menuRot, time1 * 10f);
        print(t);
    }
}
