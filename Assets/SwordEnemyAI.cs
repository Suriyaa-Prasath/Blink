using FMODUnity;  // Import FMOD namespace
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordEnemyAI : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public float sightRange = 10f;  // Range for detecting the player
    public float attackRange = 2f;  // Range to start attacking the player
    public float speed = 3f;  // Movement speed of the enemy
    public float footstepInterval = 0.5f;  // Interval between footstep sounds when running
    public float idleFootstepInterval = 2f; // Interval between footstep sounds when idle
    private float footstepTimer = 0f;  // Timer for running footsteps
    private float idleFootstepTimer = 0f;  // Timer for idle footsteps

    // FMOD event references
    public EventReference footstepSound; // FMOD event for running footstep sound
    public EventReference idleFootstepSound; // FMOD event for idle footstep sound
    public EventReference slashSound; // FMOD event for slash sound

    public float rayDistance = 2f;  // Distance to check for obstacles
    public float slashDamage =12f;  // Damage dealt by the enemy's slash
    public LayerMask whatIsPlayer;  // Layer to detect the player
    public float slashCooldown = 1.1f;  // Time between slashes in seconds

    private bool isPlayerInSightRange;  // Is the player within sight range
    private bool isPlayerInAttackRange;  // Is the player within attack range
    private bool isPlayerAlive = true;  // Is the player alive
    private bool canslash = true;  // Can the enemy slash?
    private float slashCooldownTimer;  // Timer to manage slash cooldown
    private Animator animator;  // Reference to the enemy's animator
    private PlayerHealth playerHealth;  // Reference to the PlayerHealth component

    private void Start()
    {
        // Get the Animator component attached to the enemy GameObject
        animator = GetComponent<Animator>();
        // Assuming player has a PlayerHealth component
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        // Check if the player is within sight or attack range
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Manage the slash cooldown
        if (!canslash)
        {
            slashCooldownTimer -= Time.deltaTime;
            if (slashCooldownTimer <= 0f)
            {
                canslash = true;  // Ready to slash again
            }
        }

        // State management based on player's position
        if (!isPlayerInSightRange && !isPlayerInAttackRange && isPlayerAlive)
        {
            Idle();  // No player in sight
        }
        else if (isPlayerInSightRange && !isPlayerInAttackRange && isPlayerAlive)
        {
            ChasePlayer();  // Chase the player
        }
        else if (isPlayerInSightRange && isPlayerInAttackRange && isPlayerAlive)
        {
            AttackPlayer();  // Attack the player
        }

        // Check if the player is dead
        if (playerHealth != null && playerHealth.currentHealth <= 0f && isPlayerAlive)
        {
            PlayerDied();
        }

        // Play sounds based on the enemy's state
        PlayFootstepSound();
        PlayIdleFootstepSound();
    }

    private void PlayFootstepSound()
    {
        if (animator.GetBool("isRunning"))  // If the enemy is running
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                FMODUnity.RuntimeManager.PlayOneShot(footstepSound, transform.position);
                footstepTimer = 0f;  // Reset the footstep timer
            }
        }
    }

    private void PlayIdleFootstepSound()
    {
        if (animator.GetBool("isIdle"))  // If the enemy is idle
        {
            idleFootstepTimer += Time.deltaTime;
            if (idleFootstepTimer >= idleFootstepInterval)
            {
                FMODUnity.RuntimeManager.PlayOneShot(idleFootstepSound, transform.position);
                idleFootstepTimer = 0f;  // Reset the idle footstep timer
            }
        }
    }

    private void Idle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isslashing", false);
    }

    private void ChasePlayer()
    {
        if (isPlayerInAttackRange) return;

        // Cast a ray forward to detect obstacles
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // Perform raycasts in multiple directions to avoid obstacles
        bool isObstacleAhead = Physics.Raycast(transform.position, forward, out hit, rayDistance) && hit.collider.CompareTag("Obstacle");
        bool isObstacleLeft = Physics.Raycast(transform.position, -transform.right, out hit, rayDistance) && hit.collider.CompareTag("Obstacle");
        bool isObstacleRight = Physics.Raycast(transform.position, transform.right, out hit, rayDistance) && hit.collider.CompareTag("Obstacle");

        if (isObstacleAhead || isObstacleLeft || isObstacleRight)
        {
            Reroute(isObstacleAhead, isObstacleLeft, isObstacleRight);
            return;
        }

        // Continue chasing the player if no obstacle is detected
        animator.SetBool("isRunning", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isslashing", false);

        // Move the enemy toward the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Face the player
        transform.LookAt(player);
    }

    private void Reroute(bool isObstacleAhead, bool isObstacleLeft, bool isObstacleRight)
    {
        if (isObstacleAhead)
        {
            if (!isObstacleLeft)
            {
                transform.position += -transform.right * speed * Time.deltaTime;
            }
            else if (!isObstacleRight)
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
            else
            {
                transform.position += -transform.forward * speed * Time.deltaTime;
            }
        }
        else if (isObstacleLeft)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else if (isObstacleRight)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }

        Debug.Log("Rerouting around obstacle");
    }

    private void AttackPlayer()
    {
        if (isPlayerAlive && playerHealth != null)
        {
            if (playerHealth.currentHealth <= 0f)
            {
                isPlayerAlive = false;
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
                animator.SetBool("isslashing", false);
            }
            else
            {
                animator.SetBool("isslashing", true);
            }
        }

        // Ensure the enemy stops moving when attacking
        animator.SetBool("isRunning", false);
    }

    public void slashPlayer()
    {
        if (playerHealth != null && isPlayerInSightRange && isPlayerInAttackRange && isPlayerAlive)
        {
            playerHealth.TakeDamage(slashDamage);
            FMODUnity.RuntimeManager.PlayOneShot(slashSound, transform.position);
        }
    }

    private void PlayerDied()
    {
        isPlayerAlive = false;
        animator.SetBool("isslashing", false);
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);

        Debug.Log("Player is dead");
        SceneManager.LoadScene("Game Over");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);

        // Draw the sight range sphere (green wireframe sphere)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw the attack range sphere (yellow wireframe sphere)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
