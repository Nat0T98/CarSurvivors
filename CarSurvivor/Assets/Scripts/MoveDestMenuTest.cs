using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDestMenuTest : MonoBehaviour
{
    public float time = 3;
    public Vector3 otherDest;
    private Vector3 startDest;
    private float timer;
    private bool posorneg = true;
    // Start is called before the first frame update
    void Start()
    {
        startDest = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (posorneg)
        {
            timer += Time.deltaTime;
            if (timer >= time) posorneg = false;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0) posorneg = true;
        }
        transform.position = Vector3.Lerp(otherDest, startDest, timer / time);
    }

}
