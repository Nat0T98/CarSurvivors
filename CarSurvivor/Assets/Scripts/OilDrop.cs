using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrop : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject oilFlatObj;
    public float puddleScale = 5.0f;
    public float puddleScaleVarience = 2.0f;
    public float lifeTime = 10f;
    public float dissapearTime = 2f;
    public float verticalLaunchMin = 1f;
    public float verticalLaunchMax = 1.5f;
    public float horizontalLaunchMinMax = 0.5f;
    public float launchForce = 10f;
    private float time;
    private Vector3 endScale;
    private Vector3 flatStartScale;
    private bool alreadyDisabled;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchDroplet();
        flatStartScale = oilFlatObj.transform.localScale;
    }

    private void OnEnable()
    {
        time = 0f;
        if (alreadyDisabled)
        {
            GetComponent<MeshRenderer>().enabled = true;
            oilFlatObj.GetComponent<MeshRenderer>().enabled = false;
            rb.constraints = RigidbodyConstraints.None;
            oilFlatObj.transform.localScale = flatStartScale;
            LaunchDroplet();
        }
    }

    private void Update()
    {
        TimeTillDestroy();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //print("HITIITIITITITTTTT");
        Vector3 normal = collision.contacts[0].normal;
        Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, normal);
        PuddleHit(targetRot);
    }

    void PuddleHit(Quaternion angle)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<MeshRenderer>().enabled = false;
        oilFlatObj.GetComponent<MeshRenderer>().enabled = true;
        oilFlatObj.transform.rotation = angle;

        float scalar = Random.Range(-puddleScaleVarience, puddleScaleVarience);
        float scalarResult = puddleScale - scalar;
        oilFlatObj.transform.localScale = new Vector3(scalarResult, oilFlatObj.transform.localScale.y, scalarResult);
        endScale = oilFlatObj.transform.localScale;
    }
    
    void TimeTillDestroy()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            float scalar = (time - lifeTime) / dissapearTime;
            oilFlatObj.transform.localScale = Vector3.Lerp(endScale, new Vector3(0, oilFlatObj.transform.localScale.y, 0), scalar);
            if (time >= lifeTime + dissapearTime)
            {
                alreadyDisabled = true;
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
    }

    void LaunchDroplet()
    {
        float randX = Random.Range(-horizontalLaunchMinMax, horizontalLaunchMinMax);
        float randZ = Random.Range(-horizontalLaunchMinMax, horizontalLaunchMinMax);
        float randY = Random.Range(verticalLaunchMin, verticalLaunchMax);
        Vector3 completeVec = new Vector3(randX, randY, randZ);
        Vector3 normalizedVec = completeVec.normalized;
        rb.AddForce(normalizedVec * launchForce, ForceMode.Impulse);
        print("launched");
    }
}
