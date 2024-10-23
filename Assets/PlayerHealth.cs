using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // Max health of the player
    public float currentHealth;    // Current health of the player

    void Start()
    {
        currentHealth = maxHealth;  // Initialize health
    }

    // Method to reduce the player's health
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Deduct health

        Debug.Log("Player Health: " + currentHealth);

        // Check if the player is dead
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Player dies if health reaches zero
    void Die()
    {
        Debug.Log("Player died");
        // Add logic for what happens when the player dies
        Destroy(gameObject);  // Destroy the player GameObject (optional)
    }
}

