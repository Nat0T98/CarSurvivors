using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleBreak : MonoBehaviour
{
    private bool startCount;
    private bool isShrink;
    private Vector3 startScale;
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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("CarAi"))
        {
            if (!isShrink)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                startCount = true;
                StartCoroutine(ShrinkCor());
            }
        }
    }

   

    IEnumerator ShrinkCor()
    {
        isShrink = true;
        yield return new WaitForSeconds(timeBeforeShrink);
        float elapsedTime = 0f;
        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsedTime / shrinkDuration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
