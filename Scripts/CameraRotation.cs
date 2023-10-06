using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 60.0f;
    public float forwardForce = 100.0f;
    public Text CounterText;
    private int starCount;
    private int planetCount;
    private int moonCount;
    private Rigidbody rb;

    private Quaternion targetRotation;
    private float smoothness = 5.0f;

    public AudioSource hitSound;
    private AudioSource spaceShipSound;

    private bool isSpacePressed = false;

    void Start()
    {
        starCount = 0;
        planetCount = 0;
        moonCount = 0;
        rb = GetComponent<Rigidbody>();
        hitSound = GetComponent<AudioSource>();
        spaceShipSound = GameObject.Find("SpaceShip").GetComponent<AudioSource>();
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
        if (Input.GetKeyDown(KeyCode.Space) && !isSpacePressed)
        {
            StartCoroutine(PlaySpaceShipSoundAndWait());
        }
        if (Input.GetKey(KeyCode.Space))
        {
            {
                // Apply a forward force to the camera's rigidbody
                Vector3 forwardForceVector = transform.forward * forwardForce;
                rb.AddForce(forwardForceVector, ForceMode.Force);
            }
        }
    }

    private IEnumerator PlaySpaceShipSoundAndWait()
    {
        // Set a flag to prevent multiple coroutines from being started
        isSpacePressed = true;

        // Play the spaceship sound
        spaceShipSound.Play();

        // Wait until the sound has finished playing
        yield return new WaitForSeconds(spaceShipSound.clip.length);

        // Reset the flag to allow playing the sound again
        isSpacePressed = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet") || collision.gameObject.CompareTag("Moon") || collision.gameObject.CompareTag("Star"))
        {
            hitSound.Play();
            // Calculate the impact force direction (opposite of collision normal)
            Vector3 impactDirection = -collision.contacts[0].normal;

            // Calculate impact force based on the mass of the collided object (planet)
            float impactForce = rb.mass * collision.gameObject.GetComponent<Rigidbody>().mass; // Adjust the force as needed

            // Apply an impact force to the camera
            rb.AddForce(impactDirection * impactForce, ForceMode.Impulse);

            // Increment your count or handle the impact here
            if (collision.gameObject.CompareTag("Planet"))
            {
                planetCount += 1;
            }
            else if (collision.gameObject.CompareTag("Star"))
            {
                starCount += 1;
            }
            else
            {
                moonCount += 1;
            }
            CounterText.text = "Star: " + starCount + "\n" + "Planet : " + planetCount + "\n" + "Moon : " + moonCount + "\n";
        }
    }

}
