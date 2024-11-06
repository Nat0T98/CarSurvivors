using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSmoke : MonoBehaviour
{
    public float lifetime = 3f;
    public float lifetimeRandVarience = 0.5f;
    public float moveSpeed = 100f;
    public float startScale = 0.3f;
    public float startScaleVarience = 0.1f;
    public float endScale = 1f;
    public float endScaleVarience = 0.3f;
    public float scaleSpeed = 1;
    private float randomScaleMin;
    private float randomScaleMax;
    private float scalar;
    private float time;
    private float randomLifeResult;
    private Vector3 randDirection;
    private Vector3 spawnPos;
    
    // Start is called before the first frame update
    void Start()
    {
        randomScaleMin = startScale + Random.Range(-startScaleVarience, startScaleVarience);
        randomScaleMax = endScale + Random.Range(-endScaleVarience, endScaleVarience);
        randomLifeResult += Random.Range(-lifetimeRandVarience, lifetimeRandVarience);
        randDirection = new Vector3 (Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        spawnPos = transform.position;

        scalar = randomScaleMin;
        gameObject.transform.localScale = new Vector3(scalar, scalar, scalar);
    }

    // Update is called once per frame
    void Update()
    {
        RandMove();
        DelayDestroy();
    }

    void DelayDestroy()
    {
        time += Time.deltaTime;
        if (time > lifetime + randomLifeResult)
        {
            Destroy(gameObject);
        }
    }

    void RandMove()
    {
        gameObject.transform.position = spawnPos + randDirection * time * moveSpeed;

        scalar = Mathf.Lerp (randomScaleMin, randomScaleMax, time * scaleSpeed);
        gameObject.transform.localScale = new Vector3(scalar, scalar, scalar);
    }
}
