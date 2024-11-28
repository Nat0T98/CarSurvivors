using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCollisionBox : MonoBehaviour
{
    public GameObject car;
    private CarPlayer player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = car.GetComponent<CarPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.camMove = true;
        }
    }
}
