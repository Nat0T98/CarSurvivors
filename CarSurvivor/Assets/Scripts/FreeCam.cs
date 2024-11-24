using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float moveSpeed = 10f;        
    public float fastMoveSpeed = 50f;   
    public float rotationSpeed = 3f;    
    public bool lockCursor = true;      

    private float yaw = 0f;             
    private float pitch = 0f;           

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;  
            Cursor.visible = false;                   
        }
    }

    void Update()
    {
        
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f); 

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        
        float speed = Input.GetKey(KeyCode.LeftShift) ? fastMoveSpeed : moveSpeed;
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),   
            0f,                            
            Input.GetAxis("Vertical")      
        );

        
        if (Input.GetKey(KeyCode.E))
        {
            move.y += 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            move.y -= 1f;
        }

        transform.Translate(move * speed * Time.deltaTime, Space.Self);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockCursor = !lockCursor;
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
    }
}
