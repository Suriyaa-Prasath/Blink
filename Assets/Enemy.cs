using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    // Method to call when the enemy dies
    public void Die()
    {
        animator.SetTrigger("Die"); // Trigger the death animation
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
        Destroy(gameObject);
    }
}
