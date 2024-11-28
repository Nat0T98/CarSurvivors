using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MenuCam : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    public float moveTime = 3f;
    public float endMoveTime = 3f;
    public Vector3 menuPos;
    public Quaternion menuRot;
    public Vector3 endPos;
    public Quaternion endRot;
    public Vector3 finalPos;

    public GameObject car;
    private bool shouldLook;

    private float time1;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation; //.eulerAngles;
        StartCoroutine(StartMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLook)
        {
            transform.LookAt(car.transform);
        }
    }

    //void StartMove()
    //{
    //    time1 += Time.deltaTime;
    //    float t = Mathf.Clamp01(time1 / moveTime);
    //    float a = Mathf.SmoothStep(0, 1 , t);
    //    transform.position = Vector3.Slerp(startPos, menuPos, a);
    //    transform.rotation = Quaternion.Slerp(startRot, menuRot, a);
    //    //transform.rotation = Quaternion.RotateTowards(startRot, menuRot, time1 * 10f);
    //    //print(t);
    //}

    IEnumerator StartMove()
    {
        float time1 = 0f;
        while (time1 < moveTime)
        {
            time1 += Time.deltaTime;
            float t = Mathf.Clamp01(time1 / moveTime);
            float a = Mathf.SmoothStep(0, 1, t);
            transform.position = Vector3.Slerp(startPos, menuPos, a);
            transform.rotation = Quaternion.Slerp(startRot, menuRot, a);

            yield return null;
        }

        transform.position = menuPos;
        transform.rotation = menuRot;
    }

    public IEnumerator EndMove()
    {
        shouldLook = true;
        float time1 = 0f; // Ensure the timer starts at 0
        while (time1 < endMoveTime)
        {
            time1 += Time.deltaTime;
            transform.position = Vector3.Slerp(menuPos, finalPos, time1 / endMoveTime);
            
            yield return null;
        }

    }
}
