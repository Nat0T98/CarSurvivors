using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbWireSquish : MonoBehaviour
{
    private bool doOnce;
    private Vector3 startScale;
    private Vector3 startRot;
    private Vector3 startPos;
    public float shrinkTime = 0.5f;
    public float squishPercent = 0.2f;
    public float goDown = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        startRot = transform.eulerAngles;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CarAi"))
        {
            if (!doOnce)
            {
                StartCoroutine(Squish());
            }
        }
    }

    IEnumerator Squish()
    {
        doOnce = true;
        float elapsedTime = 0f;
        while (elapsedTime < shrinkTime)
        {
            elapsedTime += Time.deltaTime;
            gameObject.transform.rotation = Quaternion.Euler(startRot.x, startRot.y, Mathf.Lerp(startRot.z, 0f, elapsedTime / shrinkTime));
            gameObject.transform.localScale = new Vector3(startScale.x, Mathf.Lerp(startScale.y, startScale.y * squishPercent, elapsedTime / shrinkTime), startScale.z);
            gameObject.transform.position = new Vector3(startPos.x, Mathf.Lerp(startPos.y, startPos.y - goDown, elapsedTime / shrinkTime), startPos.z);
            yield return null;
        }
    }
}
