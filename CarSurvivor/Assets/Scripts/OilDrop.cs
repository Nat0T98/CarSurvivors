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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchDroplet();
    }

    // Update is called once per frame
    void Update()
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

        float scalar = Random.RandomRange(-puddleScaleVarience, puddleScaleVarience);
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
                Destroy(gameObject);
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
    }
}
