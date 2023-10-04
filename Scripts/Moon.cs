using UnityEngine;

public class Moon : CelestialBody
{
    public Transform planetToOrbit;
    private float moonSpeed;

    protected override void Start()
    {
        base.Start();
        // Add any additional moon-specific initialization code here
        moonSpeed = Random.Range(-1f, .3f); // Set a random moon speed range as needed
    }

    protected override void Update()
    {
        base.Update();
        // Implement moon's circular movement around its parent planet here
        if (planetToOrbit != null)
        {
            Vector3 newPosition = planetToOrbit.position;
            newPosition.x += Mathf.Cos(Time.time * moonSpeed) * orbitRadius;
            newPosition.z += Mathf.Sin(Time.time * moonSpeed) * orbitRadius;
            transform.position = newPosition;
        }
    }
}
