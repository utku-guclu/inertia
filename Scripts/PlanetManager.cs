using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public Planet[] planetPrefabs;
    public Moon[] moonPrefabs;
    public Star[] starPrefabs;
    public float maxSpawnDistance = 100000f;
    public float altitudeRange = 10000f;
    private const float G = 6.67430f;

    private void Start()
    {
        // Call the galaxy() function to create random galaxies
            galaxy();
    }


    private Vector3 GetRandomPosition(float maxDistance)
    {
        float randomX = Random.Range(-maxDistance, maxDistance);
        float randomZ = Random.Range(-maxDistance, maxDistance);
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

        // Apply gravitational forces between stars and other celestial bodies
        foreach (Star star in FindObjectsOfType<Star>())
        {
            foreach (CelestialBody celestialBody in FindObjectsOfType<CelestialBody>())
            {
                if (celestialBody is Star || celestialBody is Planet || celestialBody is Moon)
                {
                    Vector3 force = CalculateGravitationalForce(star, celestialBody);
                    celestialBody.GetComponent<Rigidbody>().AddForce(force);
                }
            }
        }
    }

    private void galaxy()
    {
        // Instantiate stars
        foreach (Star starPrefab in starPrefabs)
        {
            // Define a new maxSpawnDistance for stars, which is smaller
            float starMaxSpawnDistance = Random.Range(maxSpawnDistance * 0.5f, maxSpawnDistance); // Adjust the range as needed

            // Instantiate stars at random positions within starMaxSpawnDistance
            Vector3 starPosition = GetRandomPosition(starMaxSpawnDistance) * Random.Range(-10, 10);
            // Ensure stars are not too close to existing stars
            bool tooClose = IsStarTooClose(starPosition, starMaxSpawnDistance);
            // Retry if the star is too close to existing stars
            while (tooClose)
            {
                starPosition = GetRandomPosition(starMaxSpawnDistance);
                tooClose = IsStarTooClose(starPosition, starMaxSpawnDistance);
            }

            Star newStar = Instantiate(starPrefab, starPosition, Quaternion.identity);

            foreach (Planet planetPrefab in planetPrefabs)
            {
                Planet newPlanet = Instantiate(planetPrefab, newStar.transform.position + GetRandomPosition(maxSpawnDistance), Quaternion.identity);
                // Set planet-specific properties here
                newPlanet.starToOrbit = newStar.transform;

                int numberOfMoonsPerPlanet = Random.Range(1, moonPrefabs.Length); // Set the desired number of moons per planet
                                                                                  // Instantiate moons for this planet
                for (int j = 0; j < numberOfMoonsPerPlanet; j++)
                {
                    // Instantiate a random moon prefab from the array
                    Moon randomMoonPrefab = moonPrefabs[Random.Range(0, moonPrefabs.Length)];
                    Moon newMoon = Instantiate(randomMoonPrefab, newPlanet.transform.position + GetRandomPosition(maxSpawnDistance), Quaternion.identity);

                    // Set moon-specific properties here
                    newMoon.planetToOrbit = newPlanet.transform;
                }
            }
        }

        // Apply gravitational forces between planets
        ApplyGravitationalForces();
    }
    private bool IsStarTooClose(Vector3 position, float minDistance)
    {
        // Check if the position is too close to any existing star
        foreach (Star star in FindObjectsOfType<Star>())
        {
            if (Vector3.Distance(position, star.transform.position) < minDistance)
            {
                return true;
            }
        }
        return false;
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

    private Vector3 CalculateGravitationalForce(CelestialBody body1, CelestialBody body2)
    {
        Vector3 direction = body2.transform.position - body1.transform.position;
        float distance = direction.magnitude;

        // Check for division by zero or extremely small distances
        if (distance < 0.001f)
        {
            return Vector3.zero; // Return zero force or any other appropriate value
        }

        float forceMagnitude = (G * body1.GetComponent<Rigidbody>().mass * body2.GetComponent<Rigidbody>().mass) / (distance * distance);
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