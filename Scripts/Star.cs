using UnityEngine;

public class Star : CelestialBody
{
    public float rotationSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Set random values for star-specific properties
        rotationSpeed = Random.Range(1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        // Implement star-specific behavior here
        RotateStar();
    }

    private void RotateStar()
    {
        // Rotate the star around its own axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
