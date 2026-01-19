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
    
    public float maxBoostFuel = 100f;
    public float currentBoostFuel;
    public float boostFuelConsumption = 10f;
    public float boostFuelRegeneration = 1f;
    public float boostFuelCooldownTimer = 0f;
    public float boostFuelCooldownDuration = 2f;

    [SerializeField] private EngineSystem engineSystem;
    [SerializeField] private VolumeProfileManager volumeProfileManager;
    [SerializeField] private HUDManager hudManager;

    private Rigidbody _rigidbody;

    private void Start()
    {
        currentBoostFuel = maxBoostFuel;
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        var shiftInput = Input.GetKey(KeyCode.LeftShift);
        
        if (verticalInput > 0f && shiftInput && currentBoostFuel > 0f)
        {
            boostActive = true;
            currentBoostFuel -= boostFuelConsumption * Time.deltaTime;
        }
        else if (currentBoostFuel <= 0f)
        {
            boostActive = false;
            if (boostFuelCooldownTimer <= 0)
            {
                hudManager.SetBoostSymbol(true);
            }
            boostFuelCooldownTimer += Time.deltaTime;
            if (boostFuelCooldownTimer >= boostFuelCooldownDuration)
            {
                currentBoostFuel += boostFuelRegeneration * Time.deltaTime;
                boostFuelCooldownTimer = 0f;
                hudManager.SetBoostSymbol(false);
            }
        }
        else
        {
            boostActive = false;
            currentBoostFuel += boostFuelRegeneration * Time.deltaTime;
        }
        volumeProfileManager.AddChroma(Time.deltaTime * (boostActive ? 2f : -2f));
        currentBoostFuel = Mathf.Clamp(currentBoostFuel, 0f, maxBoostFuel);
        hudManager.SetFuelFillBar(currentBoostFuel / maxBoostFuel);

        _rigidbody.AddRelativeForce(Vector3.forward * (speed * verticalInput * (boostActive ? boostSpeed : 1)));
        _rigidbody.AddRelativeForce(Vector3.right * (turnSpeed * horizontalInput));
        
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        
        engineSystem.SetFuelEmission(verticalInput , boostActive);

        if (!Input.GetKey(KeyCode.Space))
        {
            xRotation -= mouseY;
            yRotation -= mouseX;
        }

        xRotation = Mathf.Clamp(xRotation, -1f, 1f);
        yRotation = Mathf.Clamp(yRotation, -1f, 1f);

        _rigidbody.AddRelativeTorque(Vector3.forward * (yRotation * torqueSpeed));
        _rigidbody.AddRelativeTorque(Vector3.right * (xRotation * torqueSpeed));

        if(!Input.GetKey(KeyCode.Space))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            var planet = other.transform.parent.name;
            Debug.Log($"Crashed into {planet}");
            hudManager.GameOver($"Crashed into {planet}");
            Destroy(gameObject);
        }
    }
}
