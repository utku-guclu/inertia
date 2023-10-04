using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public Planet[] planetPrefabs;
    public float maxSpawnDistance = 60f;
    public Moon[] moonPrefabs;
    public float altitudeRange = 60f;
    public float minOrbitRadius = 15f;
    public float maxOrbitRadius = 60f;
    private const float G = 6.67430f;

    private void Start()
    {
        int numberOfPlanets = planetPrefabs.Length;
        // Instantiate planets
        for (int i = 0; i < numberOfPlanets; i++)
        {
            // Instantiate a random planet prefab from the array
            Planet randomPlanetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
            Planet newPlanet = Instantiate(randomPlanetPrefab, GetRandomPosition(), Quaternion.identity);

            // Set planet-specific properties here
            int numberOfMoonsPerPlanet = Random.Range(1, moonPrefabs.Length); // Set the desired number of moons per planet
            // Instantiate moons for this planet
            for (int j = 0; j < numberOfMoonsPerPlanet; j++)
            {
                // Instantiate a random moon prefab from the array
                Moon randomMoonPrefab = moonPrefabs[Random.Range(0, moonPrefabs.Length)];
                Moon newMoon = Instantiate(randomMoonPrefab, newPlanet.transform.position + GetRandomPosition(), Quaternion.identity);

                // Set moon-specific properties here
                newMoon.planetToOrbit = newPlanet.transform;
                newMoon.orbitSpeed = Random.Range(1f, 15f); // Adjust the orbit speed as needed
            }
        }

        // Apply gravitational forces between planets
        ApplyGravitationalForces();
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-maxSpawnDistance, maxSpawnDistance);
        float randomZ = Random.Range(-maxSpawnDistance, maxSpawnDistance);
        float altitude = Random.Range(-altitudeRange, altitudeRange);
        return new Vector3(randomX, altitude, randomZ);
    }

    private void ApplyGravitationalForces()
    {
        // Apply gravitational forces between planets
        foreach (Planet planet1 in FindObjectsOfType<Planet>())
        {
            foreach (Planet planet2 in FindObjectsOfType<Planet>())
            {
                if (planet1 != planet2)
                {
                    Vector3 force = CalculateGravitationalForce(planet1, planet2);
                    planet1.GetComponent<Rigidbody>().AddForce(force);
                }
            }
        }

        // Apply gravitational forces between moons and planets
        foreach (Moon moon in FindObjectsOfType<Moon>())
        {
            foreach (Planet planet in FindObjectsOfType<Planet>())
            {
                Vector3 force = CalculateGravitationalForce(moon, planet);
                moon.GetComponent<Rigidbody>().AddForce(force);
            }
        }
    }

    private Vector3 CalculateGravitationalForce(Planet planet1, Planet planet2)
    {
        Vector3 direction = planet2.transform.position - planet1.transform.position;
        float distance = direction.magnitude;
        float forceMagnitude = (G * planet1.GetComponent<Rigidbody>().mass * planet2.GetComponent<Rigidbody>().mass) / (distance * distance);
        Vector3 force = direction.normalized * forceMagnitude;
        return force;
    }

    // Separate method for calculating gravitational forces between moons and planets
    private Vector3 CalculateGravitationalForce(Moon moon, Planet planet)
    {
        Vector3 direction = planet.transform.position - moon.transform.position;
        float distance = direction.magnitude;
        float forceMagnitude = (G * moon.GetComponent<Rigidbody>().mass * planet.GetComponent<Rigidbody>().mass) / (distance * distance);
        Vector3 force = direction.normalized * forceMagnitude;
        return force;
    }
    // Function to apply an impact force when two planets collide
    public void ApplyImpactForce(Planet planet1, Planet planet2, float impactForce)
    {
        // Calculate the direction from planet1 to planet2
        Vector3 impactDirection = planet2.transform.position - planet1.transform.position;

        // Apply the impact force to both planets
        planet1.GetComponent<Rigidbody>().AddForce(impactDirection.normalized * impactForce);
        planet2.GetComponent<Rigidbody>().AddForce(-impactDirection.normalized * impactForce);
    }
}