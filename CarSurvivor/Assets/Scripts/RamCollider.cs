using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.player.GetComponent<MainCar>().ramTriggerEnter(other);
    }
}
