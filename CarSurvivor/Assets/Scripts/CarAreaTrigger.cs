using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.player.GetComponent<MainCar>().areaTriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.player.GetComponent<MainCar>().areaTriggerExit(other);
    }
}
