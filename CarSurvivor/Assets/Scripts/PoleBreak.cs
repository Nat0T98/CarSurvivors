using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleBreak : MonoBehaviour
{
    private bool startCount;
    private bool startShrink;
    private Vector3 startScale;
    private float time;
    public float timeBeforeShrink = 5f;
    public float shrinkDuration = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Shrink();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("CarAi"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
            startCount = true;
        }
    }

    void Shrink()
    {
        if (startCount)
        {
            time += Time.deltaTime;
            if (timeBeforeShrink <= time)
            {
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, (time - timeBeforeShrink) / shrinkDuration);
                if (time > timeBeforeShrink + shrinkDuration)
                {
                    Destroy(gameObject);
                }
            }
        }

    }
}
