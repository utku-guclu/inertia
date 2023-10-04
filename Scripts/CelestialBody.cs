using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public Color bodyColor;
    protected float minMass = 1.0f;
    protected float maxMass = 10.0f;
    protected float randomMass;
    protected float minDrag = 0.1f;
    protected float maxDrag = 1.0f;
    protected float randomDrag;
    protected float bodySpeed = 1.0f;
    public float orbitSpeed = 1.0f;
    public float orbitRadius;
    public float bodySize;

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        bodySize = Random.Range(1f, 7f);
        orbitRadius = Random.Range(5f, 60f);

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
