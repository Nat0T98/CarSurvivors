using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTESTER : MonoBehaviour
{

    private GameObject PlayerObject;


    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            PlayerObject.GetComponent<MainCar>().MelleUpgradeTest();
        }
    }
}
