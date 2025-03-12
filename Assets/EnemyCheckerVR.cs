using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCheckerVR : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load
    private float delayBeforeNextScene = 3f; // Delay in seconds before moving to the next scene
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

    void Update()
    {
        CheckAllEnemiesDestroyed();
    }

    private void CheckAllEnemiesDestroyed()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies are found
        if (enemies.Length == 0)
        {
            // Stop the heartbeat sound
            if (playerHealth != null)
            {
                playerHealth.StopHeartbeatSound();
            }

            // Start the coroutine to move to the next scene after a delay
            StartCoroutine(LoadNextSceneAfterDelay());
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayBeforeNextScene);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
