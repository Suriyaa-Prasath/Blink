using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // Max health of the player
    public float currentHealth;    // Current health of the player
    public Slider slider;
    public Canvas InGameCanvas;
    public Canvas DeadCanvas;

    void Start()
    {
        DeadCanvas.enabled = false;
        currentHealth = maxHealth;  // Initialize health
        slider.maxValue = maxHealth;
    }

    // Method to reduce the player's health
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Deduct health

        slider.value = currentHealth;
        Debug.Log(currentHealth);
        // Check if the player is dead
        if (currentHealth <= 0f)
        {
            InGameCanvas.enabled = false;
            DeadCanvas.enabled = true;
            Die();
        }
    }

    // Player dies if health reaches zero
    void Die()
    {
        Time.timeScale = 0.0f;
    }
}

