using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSmoke : MonoBehaviour
{
    public float randomScaleRange = 1;
    public float lifetime = 3f;
    public float lifetimeRandVarience = 0.5f;
    private float time;
    private float randomLifeResult;
    
    // Start is called before the first frame update
    void Start()
    {
        float random = Random.Range(-randomScaleRange, randomScaleRange);
        transform.localScale += new Vector3(random, random, random);
        randomLifeResult += Random.Range(-lifetimeRandVarience, lifetimeRandVarience);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > lifetime + randomLifeResult)
        {
            Destroy(gameObject);
        }
    }
}
