using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDrop : MonoBehaviour
{
    public float notNearPlayerLaunchStrength = 1.0f;
    public float distanceToPlayerLaunch;
    public float randLaunchValue = 1.0f;
    private Rigidbody rb;
    private int randLimb;
    private bool alreadyDisabled;
    [Serializable]
    public struct Limbs
    {
        public GameObject limbObj;
    }
    public List<Limbs> limbs;
    public float timeBeforeShrink = 5;
    public float shrinkDuration = 2;
    private Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        EnableRandom();
        LaunchObject();
        StartCoroutine(ShrinkCor(gameObject));
    }

    private void OnEnable()
    {
        if (alreadyDisabled)
        {
            transform.localScale = startScale;
            EnableRandom();
            LaunchObject();
            StartCoroutine(ShrinkCor(gameObject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableRandom()
    {
        randLimb = UnityEngine.Random.Range(0, limbs.Count);
        limbs[randLimb].limbObj.SetActive(true);
    }

    void LaunchObject()
    {
        Vector3 velo;
        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) <= distanceToPlayerLaunch)
        {
            velo = GameManager.Instance.player.GetComponent<Rigidbody>().velocity;
            notNearPlayerLaunchStrength = 1f;
        }
        else
        {
            velo = Vector3.zero;
        }
        float randX = UnityEngine.Random.Range(-randLaunchValue, randLaunchValue);
        float randY = UnityEngine.Random.Range(-randLaunchValue, randLaunchValue);
        float randZ = UnityEngine.Random.Range(-randLaunchValue, randLaunchValue);
        rb.velocity = velo + new Vector3(randX, randY, randZ) * notNearPlayerLaunchStrength;
    }

    IEnumerator ShrinkCor(GameObject obj)
    {
        yield return new WaitForSeconds(timeBeforeShrink);
        float elapsedTime = 0f;
        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsedTime / shrinkDuration);
            yield return null;
        }
        //Destroy(gameObject);
        limbs[randLimb].limbObj.SetActive(false);
        alreadyDisabled = true;
        gameObject.SetActive(false);
    }
}
