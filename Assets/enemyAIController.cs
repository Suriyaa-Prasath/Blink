using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, obstacleMask;
    public float health;
    public Animator animator;

    public bool meleeDamageActive = false;
    public bool bulletDamageActive = false;

    private bool isPlayerAlive = true;
    private bool canPunch = true;
    public float punchCooldown = 1.1f;
    private PlayerHealth playerHealth;
    public int meleeDamage = 10;
    public int bulletDamage = 20;

    // Patrolling
    public float patrolDistance = 10f;
    public Vector3 initialPosition;
    private Vector3 patrolTarget;
    private bool movingRight = true;
    public bool X = true;

    // Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectile; // Prefab of the projectile
    public Transform firePoint; // Position where the bullet is spawned

    // States
    public float sightRange, attackRange, meleeRange;
    private bool playerInSightRange, playerInAttackRange, playerInMeleeRange;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();

        // Freeze rotation on X and Z axes to avoid tilting
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }

        initialPosition = transform.position;
        SetPatrolTarget();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInMeleeRange = Physics.CheckSphere(transform.position, meleeRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !playerInMeleeRange)
        {
            Patrol();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            FollowPlayer();
        }
        else if (playerInAttackRange && !playerInMeleeRange)
        {
            AttackPlayer(); // Ranged attack
        }
        else if (playerInMeleeRange)
        {
            MeleeAttackPlayer(); // Melee attack if in melee range
        }

        // Lock rotation to only allow Y-axis rotation
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Patrol()
    {
        bulletDamageActive = false;
        meleeDamageActive = false;

        animator.SetBool("PlayerSighted", false);
        animator.SetBool("PlayerInAttackRange", false);
        animator.SetBool("PlayerMeleeDamage", false);

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            movingRight = !movingRight;
            SetPatrolTarget();
        }

        agent.SetDestination(patrolTarget);
    }

    private void SetPatrolTarget()
    {
        float direction = movingRight ? 1 : -1;
        if (X)
        {

            patrolTarget = initialPosition + new Vector3(patrolDistance * direction, 0, 0);
        }
        else 
        {
            patrolTarget = initialPosition + new Vector3(0, 0, patrolDistance * direction);
        }
    }

    private void FollowPlayer()
    {
        bulletDamageActive = false;
        meleeDamageActive = false;

        animator.SetBool("PlayerSighted", true);
        animator.SetBool("PlayerInAttackRange", false);
        animator.SetBool("PlayerMeleeDamage", false);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only follow the player if they're outside the attack range
        if (distanceToPlayer > attackRange)
        {
            if (!HasObstacleBetween(transform.position, player.position))
            {
                // Move towards the player if there is no obstacle
                Vector3 destination = new Vector3(player.position.x, 0, player.position.z);
                agent.SetDestination(destination);
            }
            else
            {
                // Find a new position around the player if an obstacle is detected
                FindNewPositionAroundPlayer();
            }
        }
        else
        {
            // Stop moving when the player is within attack range
            agent.isStopped = true; // Stop moving when in attack range
        }
    }

    private void FindNewPositionAroundPlayer()
    {
        float searchRadius = 20f;
        float angleStep = 15f;

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 checkPosition = player.position + direction * searchRadius;

            if (NavMesh.SamplePosition(checkPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) &&
                !HasObstacleBetween(hit.position, player.position))
            {
                agent.SetDestination(hit.position);
                break;
            }
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool("PlayerSighted", false);
        animator.SetBool("PlayerInAttackRange", true);
        animator.SetBool("PlayerMeleeDamage", false);

        bulletDamageActive = true;
        meleeDamageActive = false;


        // Stop the agent's movement while attacking
        agent.isStopped = true;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!alreadyAttacked && !playerInMeleeRange)
        {
            Shoot();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Shoot()
    {
        // Create a projectile instance at firePoint's position and rotation
        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Calculate the direction based on the firePoint's rotation
        Vector3 direction = firePoint.rotation * Vector3.forward;

        // Apply force in the local forward direction based on firePoint's rotation
        rb.AddForce(direction * 32f, ForceMode.Impulse);
    }

    private void MeleeAttackPlayer()
    {
        animator.SetBool("PlayerSighted", false);
        animator.SetBool("PlayerInAttackRange", false);
        animator.SetBool("PlayerMeleeDamage", true);

        bulletDamageActive = false;
        meleeDamageActive = true;


        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (playerHealth != null && isPlayerAlive)
        {
            canPunch = false;
            Invoke(nameof(ResetPunch), punchCooldown);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        agent.isStopped = false; // Re-enable movement after attack cooldown
    }

    private void ResetPunch()
    {
        canPunch = true;
    }

    private bool HasObstacleBetween(Vector3 start, Vector3 end)
    {
        if (Physics.Raycast(start, (end - start).normalized, out RaycastHit hit, Vector3.Distance(start, end), obstacleMask))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Obstacle detected between enemy and player: " + hit.collider.gameObject.name);
                return true;
            }
        }
        return false;
    }

    private void DealDamage()
    {
        Debug.Log(meleeDamageActive); 
        if (playerHealth != null && bulletDamageActive && !meleeDamageActive && isPlayerAlive)
        {
            playerHealth.TakeDamage(bulletDamage);
            Debug.Log(playerHealth.currentHealth.ToString());
        }
        else if (playerHealth != null && !bulletDamageActive && meleeDamageActive && isPlayerAlive)
        {
            Debug.Log(playerHealth.currentHealth.ToString());
            playerHealth.TakeDamage(meleeDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * 5f);
        }
    }
}
