using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;             // Reference to the player's transform
    public float detectionRange = 10f;   // How far the enemy can detect the player
    public float speed = 3f;             // Enemy movement speed
    public float attackRange = 1.5f;     // Distance to player at which enemy will punch
    public int damage = 20;              // Damage dealt by the enemy's punch

    private Animator animator;           // Reference to the Animator component
    private bool isPlayerDetected = false; // Flag to check if the player has been detected
    private bool isAttacking = false;    // Flag to prevent multiple attacks while attacking

    void Start()
    {
        animator = GetComponent<Animator>();  // Get the Animator component
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned! Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        // If player is detected, chase the player
        if (isPlayerDetected && !isAttacking)
        {
            ChasePlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected within range.");
            isPlayerDetected = true; // Set player detection flag
            animator.SetBool("isRunning", true); // Switch to running animation
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("Chasing Player...");

        // Ensure player is valid
        if (player == null)
        {
            Debug.LogError("Player reference is missing.");
            return;
        }

        // Calculate the direction from enemy to player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy toward the player
        transform.position += direction * speed * Time.deltaTime;
        Debug.Log("Enemy Position: " + transform.position);

        // Rotate the enemy to face the player
        transform.LookAt(player);

        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        // If the enemy is close enough, start attacking
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Enemy is close enough to attack!");
            animator.SetBool("isRunning", false); // Stop running
            animator.SetTrigger("Punch");         // Trigger punch animation
            isAttacking = true;                   // Set attacking flag
        }
    }

    // This function will be called from an animation event in the punch animation
    public void PunchPlayer()
    {
        // Assuming the player has a "PlayerHealth" script to handle health reduction
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Debug.Log("Punching player and dealing damage.");
                playerHealth.TakeDamage(damage);  // Reduce player's health
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on player.");
            }
        }

        // Reset attacking flag to allow for future attacks after the punch animation finishes
        StartCoroutine(ResetAttack());
    }

    // Coroutine to reset the attacking flag after a short delay
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f); // Adjust based on punch animation duration
        Debug.Log("Resetting attack state.");
        isAttacking = false;                 // Reset attacking flag
    }

    // Optionally, draw the detection range in the scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
