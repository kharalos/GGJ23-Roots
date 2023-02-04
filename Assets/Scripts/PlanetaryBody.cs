using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetaryBody : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float orbitSpeed = 1f;
    [SerializeField] private bool isRotating = true;
    [SerializeField] private bool isOrbiting = true;
    // [SerializeField] private bool isGravityOn = true;
    [SerializeField] private Transform orbitAround;
    
    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if (isOrbiting)
        {
            transform.RotateAround(orbitAround.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }
    
    // private void FixedUpdate()
    // {
    //     if (isGravityOn)
    //     {
    //         Vector3 gravityUp = (transform.position - orbitAround.position).normalized;
    //         Vector3 localUp = transform.up;
    //         transform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
    //         GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
    //     }
    // }
}
