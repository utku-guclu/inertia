using UnityEngine;

public class Moon : CelestialBody
{
    public Transform planetToOrbit;

    protected override void Start()
    {
        base.Start();
        // Add any additional moon-specific initialization code here
        orbitSpeed = Random.Range(-1f, .1f); // Set a random moon speed range as needed
        orbitRadius = Random.Range(600f, 1200f);
    }

    protected override void Update()
    {
        base.Update();
        // Implement moon's circular movement around its parent planet here
        if (planetToOrbit != null)
        {
            Vector3 newPosition = planetToOrbit.position;
            newPosition.x += Mathf.Cos(Time.time * orbitSpeed) * orbitRadius;
            newPosition.z += Mathf.Sin(Time.time * orbitSpeed) * orbitRadius;
            transform.position = newPosition;
        }
    }
}
