using UnityEngine;

public class Planet : CelestialBody
{
    private Vector3 randomRotationAxis;

    protected override void Start()
    {
        base.Start();
        // Add any additional planet-specific initialization code here
        randomRotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        orbitSpeed = Random.Range(-1f, .3f);
        orbitRadius = Random.Range(30f, 90f);
    }

    protected override void Update()
    {
        base.Update();
        ApplyRandomRotation();
        CircularMovement();
    }

    private void ApplyRandomRotation()
    {
        float rotationSpeed = Random.Range(0.1f, 1f); // Adjust the range as needed
        transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
    }

    protected override void CircularMovement()
    {
        // Calculate the new position for circular movement
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Cos(Time.time * orbitSpeed) * orbitRadius;
        newPosition.z = Mathf.Sin(Time.time * orbitSpeed) * orbitRadius;

        // Update the planet's position
        transform.position = newPosition;
    }
}
