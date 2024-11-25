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
    private bool shouldFreezeTime;

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

        Vector3 move = Vector3.zero;

        
        if (Input.GetKey(KeyCode.W)) move += transform.forward;  
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;  
        if (Input.GetKey(KeyCode.A)) move -= transform.right;    
        if (Input.GetKey(KeyCode.D)) move += transform.right;    
        if (Input.GetKey(KeyCode.E)) move += transform.up;       
        if (Input.GetKey(KeyCode.Q)) move -= transform.up;       

        
        move.Normalize();
        transform.Translate(move * speed * Time.unscaledDeltaTime, Space.World);

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockCursor = !lockCursor;
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (shouldFreezeTime == false)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            shouldFreezeTime = !shouldFreezeTime;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        string screenshotName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        ScreenCapture.CaptureScreenshot(screenshotName);
        Debug.Log($"Screenshot saved as: {screenshotName}");
    }
}
