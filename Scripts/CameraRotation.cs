using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 60.0f;
    public float moveSpeed = 30.0f;
    public float forwardForce = 30.0f;
    public Text CounterText;
    private int planetCount;
    private int moonCount;
    private Rigidbody rb;

    private Quaternion targetRotation;
    private float smoothness = 5.0f;

    void Start()
    {
        planetCount = 0;
        moonCount = 0;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input for camera rotation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate rotation angles based on input
        float rotationX = verticalInput * rotationSpeed * Time.deltaTime;
        float rotationY = horizontalInput * rotationSpeed * Time.deltaTime;

        // Apply the rotation to the camera
        transform.Rotate(Vector3.right, rotationX);
        transform.Rotate(Vector3.up, rotationY);

        // Smoothly interpolate camera rotation towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);

        // Check if the space key is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            // Calculate forward movement based on camera's rotation
            {
                // Apply a forward force to the camera's rigidbody
                Vector3 forwardForceVector = transform.forward * forwardForce;
                rb.AddForce(forwardForceVector, ForceMode.Force);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet") || collision.gameObject.CompareTag("Moon"))
        {
            // Calculate the impact force direction (opposite of collision normal)
            Vector3 impactDirection = -collision.contacts[0].normal;

            // Apply an impact force to the camera
            float impactForce = 30.0f; // Adjust the force as needed
            rb.AddForce(impactDirection * impactForce, ForceMode.Impulse);

            // Increment your count or handle the impact here
            if (collision.gameObject.CompareTag("Planet"))
            {
                planetCount += 1;
            }
            else
            {
                moonCount += 1;
            }
            CounterText.text = "Planet : " + planetCount + "\n" + "Moon : " + moonCount + "\n";
        }
    }
}
