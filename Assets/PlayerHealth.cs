using UnityEngine;
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

    void Start()
    {
        currentHealth = maxHealth;  // Initialize health

        // Initialize FMOD Heartbeat Event
        heartbeatEvent = RuntimeManager.CreateInstance(heartbeatEventRef);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Deduct health
        Debug.Log($"Current Health: {currentHealth}");

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

    void PlayHeartbeatSound()
    {
        heartbeatEvent.start();
        isHeartbeatPlaying = true;
        Debug.Log("Heartbeat Sound Started");
    }

    void StopHeartbeatSound()
    {
        heartbeatEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isHeartbeatPlaying = false;
        Debug.Log("Heartbeat Sound Stopped");
    }

    void Die()
    {
        SceneManager.LoadScene(2);
    }

    void OnDestroy()
    {
        heartbeatEvent.release(); // Clean up FMOD instance
    }
}
