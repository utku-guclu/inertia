using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public Color bodyColor;
    protected float minMass = 1.0f;
    protected float maxMass = 100.0f;
    protected float randomMass;
    protected float minDrag = 0.1f;
    protected float maxDrag = 1.0f;
    protected float randomDrag;
    protected float bodySpeed;
    public float orbitSpeed;
    public float orbitRadius;
    public float bodySize;

    public float minOrbitRadius = 600f;
    public float maxOrbitRadius = 1200f;

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        bodySpeed = Random.Range(.1f, 1f);
        orbitSpeed = Random.Range(0, .01f);
        bodySize = Random.Range(1f, 16f);

        transform.localScale = new Vector3(bodySize, bodySize, bodySize);

        randomMass = Random.Range(minMass, maxMass);
        rb.mass = randomMass;

        randomDrag = Random.Range(minDrag, maxDrag);
        rb.drag = randomDrag;

        Vector3 initialVelocity = new Vector3(
            Random.Range(-bodySpeed, bodySpeed),
            Random.Range(-bodySpeed, bodySpeed),
            Random.Range(-bodySpeed, bodySpeed)
        );

        rb.velocity = initialVelocity;
    }

    protected virtual void Update()
    {
        // Implement update logic here if needed
    }

    protected virtual void CircularMovement()
    {
        // Default implementation (empty) for circular movement
    }
}
