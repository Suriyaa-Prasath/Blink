using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

        
        Debug.Log(currentHealth);
        // Check if the player is dead
        if (currentHealth <= 0f)
        {
           
            Debug.Log("GJK");
            Die();
        }
    }

    // Player dies if health reaches zero
    void Die()
    {
        SceneManager.LoadScene(2);
        
    }
}

