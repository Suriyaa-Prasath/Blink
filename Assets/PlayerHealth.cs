using UnityEngine;
using UnityEngine.UI; // For UI Image
using UnityEngine.SceneManagement;
using FMODUnity;  // FMOD Integration
using FMOD.Studio;  // FMOD Event Instance

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 60f;            // Max health of the player
    public float currentHealth;              // Current health of the player

    public EventReference heartbeatEventRef; // Assign FMOD event in Inspector
    private EventInstance heartbeatEvent;    // FMOD Event Instance
    private bool isHeartbeatPlaying = false; // To avoid multiple triggers

    public float healthThreshold = 0.4f;     // Health threshold (40%) exposed in Inspector

    public Image healthImage;                // UI Image to change color (assign in Inspector)

    public float glowSpeed = 2f;            // Speed of the glow pulsation
    public float glowIntensity = 0.5f;      // Intensity of the glow effect

    void Start()
    {
        currentHealth = maxHealth;  // Initialize health

        // Initialize FMOD Heartbeat Event
        heartbeatEvent = RuntimeManager.CreateInstance(heartbeatEventRef);

        UpdateHealthImage();  // Set initial size and color
    }

    void Update()
    {
        if (healthImage != null)
        {
            ApplyGlowEffect();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Deduct health
        Debug.Log($"Current Health: {currentHealth}");

        UpdateHealthImage();  // Update the health image size and color

        // Trigger heartbeat if health falls below threshold
        if (currentHealth <= maxHealth * healthThreshold && !isHeartbeatPlaying)
        {
            PlayHeartbeatSound();
        }

        // Stop heartbeat if health recovers
        if (currentHealth > maxHealth * healthThreshold && isHeartbeatPlaying)
        {
            StopHeartbeatSound();
        }

        // Check if the player is dead
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthImage()
    {
        if (healthImage == null)
        {
            Debug.LogWarning("Health Image is not assigned in the Inspector.");
            return;
        }

        // Calculate health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Update color with neon effect
        if (healthPercentage > 0.6f)
        {
            healthImage.color = new Color(0f, 1f, 0.5f); // Neon green
        }
        else if (healthPercentage > 0.4f)
        {
            healthImage.color = new Color(1f, 1f, 0.2f); // Neon yellow
        }
        else
        {
            healthImage.color = new Color(1f, 0.1f, 0.1f); // Neon red
        }

        // Update size (adjust width without affecting canvas)
        RectTransform rectTransform = healthImage.rectTransform;
        rectTransform.sizeDelta = new Vector2(healthPercentage * 200f, rectTransform.sizeDelta.y); // Adjust width based on percentage (200 is base width)
    }

    void ApplyGlowEffect()
    {
        // Pulsate the alpha channel for a glow effect
        float alpha = Mathf.PingPong(Time.time * glowSpeed, glowIntensity);
        Color glowColor = healthImage.color;
        glowColor.a = 0.5f + alpha; // Ensure minimum alpha for visibility
        healthImage.color = glowColor;
    }

    void PlayHeartbeatSound()
    {
        heartbeatEvent.start();
        isHeartbeatPlaying = true;
        Debug.Log("Heartbeat Sound Started");
    }

    public void StopHeartbeatSound()
    {
        heartbeatEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isHeartbeatPlaying = false;
        Debug.Log("Heartbeat Sound Stopped");
    }

    void Die()
    {
        if (isHeartbeatPlaying)
        {
            StopHeartbeatSound();
        }

        Debug.Log("Player has died. Loading next scene...");
        SceneManager.LoadScene(2);
    }

    void OnDestroy()
    {
        heartbeatEvent.release(); // Clean up FMOD instance
    }
}
