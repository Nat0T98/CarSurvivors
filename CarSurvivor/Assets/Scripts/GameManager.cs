using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject PlayerObject;
    public float speed = 1;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerObject = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (PlayerObject.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
