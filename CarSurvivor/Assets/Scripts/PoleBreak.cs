using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleBreak : MonoBehaviour
{
    private bool isShrink;
    private Vector3 startScale;
    public float timeBeforeShrink = 5f;
    public float shrinkDuration = 2f;
    public bool isMultiObject;
    
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("CarAi"))
        {
            if (!isShrink)
            {
                if (!isMultiObject)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    StartCoroutine(ShrinkCor(gameObject));
                }
                else
                {
                    MultiChild();
                }
            }
        }
    }

   

    IEnumerator ShrinkCor(GameObject obj)
    {
        isShrink = true;
        yield return new WaitForSeconds(timeBeforeShrink);
        float elapsedTime = 0f;
        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsedTime / shrinkDuration);
            yield return null;
        }
        Destroy(gameObject);
    }

    void MultiChild()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                child.parent = null;
                StartCoroutine(ShrinkCor(child.gameObject));
                StartCoroutine(ShrinkCor(gameObject));
            }
            else
            {
                Destroy(child);
                StartCoroutine(ShrinkCor(gameObject));
            }
        }
    }
}
