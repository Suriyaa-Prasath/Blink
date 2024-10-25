using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    public EventReference death;

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    // Method to call when the enemy dies
    public void Die()
    {
        animator.SetTrigger("Die"); // Trigger the death animation
        FMODUnity.RuntimeManager.PlayOneShot(death, transform.position);
        // Optionally disable further interactions
        GetComponent<Collider>().enabled = false;
        GetComponent<EnemyAI>().enabled = false; // Disable AI if applicable


        // Call the Destroy method after the animation finishes
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(2.5f);

        // Destroy the enemy GameObject
        
       // Destroy(gameObject);
    }
}
