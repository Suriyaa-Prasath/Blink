using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Bottle : MonoBehaviour
{
    [SerializeField] private GameObject brokenBottlePrefab; // Broken bottle prefab
    [SerializeField] private float minImpactSpeed = 2.0f;    // Minimum speed required to trigger the explosion
    private Vector3 lastPosition;                          // To calculate the bottle's speed
    private float currentSpeed;                            // Current speed of the bottle
    public EventReference bottle;

    private void Start()
    {
        lastPosition = transform.position; // Initialize the last position
    }

    private void Update()
    {
        // Calculate the speed based on the distance moved per frame
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position; // Update last position
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged as "Enemy"
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Debug.Log($"Bottle hit an enemy: {other.name}");

            if (currentSpeed >= minImpactSpeed)
            {
                Debug.Log("Impact speed is sufficient. Bottle explodes!");

                // Trigger the explosion
                FMODUnity.RuntimeManager.PlayOneShot(bottle, transform.position);
                Explode();

                // Destroy the enemy
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Die(); // Assuming the Enemy script has a PunchDie() method
                    Debug.Log("Enemy defeated!");
                    
                }
                else
                {
                    Debug.LogWarning("No Enemy script attached to the collided object.");
                }
            }
            else
            {
                Debug.Log("Impact speed too low. Bottle does not explode.");
            }
        }
    }

    void Explode()
    {
        Debug.Log("Start");
        // Instantiate the broken bottle prefab at the bottle's position
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, transform.position, Quaternion.identity);
        Debug.Log("Vanthuruchi");

        // Apply random velocities to the broken pieces
        BrokenBottle brokenBottleScript = brokenBottle.GetComponent<BrokenBottle>();
        if (brokenBottleScript != null)
        {
            brokenBottleScript.RandomVelocities();
        }
        Debug.Log("Broken");

        // Destroy the bottle
        Destroy(gameObject);
        Debug.Log("Katham Katham");
    }
}
