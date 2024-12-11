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
    public float randLaunchValue = 5;
    private GameObject hittingObj;

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
                hittingObj = other.gameObject;
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
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            if (child.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                child.parent = null;
                LaunchObject(rb);
                StartCoroutine(ShrinkCor(child.gameObject));
                //StartCoroutine(ShrinkCor(gameObject));
            }
            else
            {
                Destroy(child);
                StartCoroutine(ShrinkCor(gameObject));
                print("is null");
            }
        }
    }

    void LaunchObject(Rigidbody rb)
    {
        Vector3 velo = hittingObj.GetComponent<Rigidbody>().velocity;
        float randX = UnityEngine.Random.Range(-randLaunchValue, randLaunchValue);
        float randY = UnityEngine.Random.Range(-randLaunchValue, randLaunchValue);
        float randZ = UnityEngine.Random.Range(-randLaunchValue, randLaunchValue);
        rb.velocity = velo + new Vector3(randX, randY, randZ);
    }
}
