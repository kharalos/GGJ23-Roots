using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float boostSpeed = 3f;
    public float turnSpeed = 50f;
    public float verticalInput;
    public float horizontalInput;
    public bool boostActive;
    
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public float torqueSpeed = 10f;
    
    [SerializeField] private EngineSystem engineSystem;

    private Rigidbody _rigidbody;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        boostActive = Input.GetKey(KeyCode.LeftShift);

        _rigidbody.AddRelativeForce(Vector3.forward * (speed * verticalInput * (boostActive ? boostSpeed : 1)));
        _rigidbody.AddRelativeForce(Vector3.right * (turnSpeed * horizontalInput));
        
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        
        engineSystem.SetFuelEmission(verticalInput , boostActive);

        if (!Input.GetMouseButton(2))
        {
            xRotation -= mouseY;
            yRotation -= mouseX;
        }

        xRotation = Mathf.Clamp(xRotation, -1f, 1f);
        yRotation = Mathf.Clamp(yRotation, -1f, 1f);

        _rigidbody.AddRelativeTorque(Vector3.forward * (yRotation * torqueSpeed));
        _rigidbody.AddRelativeTorque(Vector3.right * (xRotation * torqueSpeed));

        if(!Input.GetMouseButton(2))
        {
            xRotation = Mathf.Lerp(xRotation, 0f, Time.deltaTime * 2f);
            yRotation = Mathf.Lerp(yRotation, 0f, Time.deltaTime * 2f);
            
            if (xRotation is < 0.01f and > -0.01f)
            {
                xRotation = 0f;
            }

            if (yRotation is < 0.01f and > -0.01f)
            {
                yRotation = 0f;
            }
        }
    }


}
