using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, obstacleMask;
    public float health;
    public Animator animator;

    // Patrolling
    public Vector3 walkPoint;
    public Vector3 initialPosition;
    bool walkPointSet;
    public float walkPointRange;
    public int direction = 1;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform firePoint; // The point from which the enemy will shoot

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        this.enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        rb.transform.position = new Vector3(initialPosition.x, 0, initialPosition.z);
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        initialPosition = transform.position;
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        Vector3 distance = transform.position - player.transform.position;


        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            animator.SetBool("PlayerSighted", true);
            animator.SetBool("PlayerInAttackRange", false);
            FollowPlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion lockedRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = lockedRotation;
        Vector3 position = transform.position;
        position.y = 0; // Replace with the desired Y value
        transform.position = position;
    }

    private void Patroling()
    {
        animator.SetBool("PlayerSighted", false);
        animator.SetBool("PlayerInAttackRange", false);
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        else
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + direction * walkPointRange, transform.position.y, transform.position.z);
        direction = -direction;

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void FollowPlayer()
    {
        animator.SetBool("PlayerSighted", true);
        animator.SetBool("PlayerInAttackRange", false);
        // Continuously adjust position to maintain clear line of sight to the player
        if (!HasObstacleBetween(transform.position, player.position))
        {
            Vector3 destination = new Vector3(player.position.x, 0.9331f, player.position.z);
            // Move directly towards the player if no obstacle is detected
            agent.SetDestination(destination);
        }
        else
        {
            // Find a new position where there's no obstacle blocking the view
            FindNewPositionAroundPlayer();
        }
    }

    private void FindNewPositionAroundPlayer()
    {
        float searchRadius = 20f; // Radius around the player to search for a new position
        float angleStep = 15f; // Angle step for finding positions

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            // Calculate a new potential position
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 checkPosition = player.position + direction * searchRadius;

            // Check if the position is navigable and has a clear line of sight
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
        transform.eulerAngles = new Vector3(0, 0, 0);
        Quaternion lockedRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = lockedRotation;
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Shoot();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Shoot()
    {
        // Instantiate the projectile at the fire point's position and rotation
        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Use the firePoint's forward direction for shooting
        Vector3 direction = new Vector3(firePoint.forward.x, 0, firePoint.forward.z);

        // Add force to the projectile in the firePoint's forward direction
        rb.AddForce(direction * 32f, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private bool HasObstacleBetween(Vector3 start, Vector3 end)
    {
        // Perform a raycast from the enemy to the player to detect obstacles
        if (Physics.Raycast(start, (end - start).normalized, out RaycastHit hit, Vector3.Distance(start, end), obstacleMask))
        {
            // Check if the hit object has the Obstacle tag
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Obstacle detected between enemy and player: " + hit.collider.gameObject.name);
                return true;
            }
        }
        return false;
    }

    private void backToOriginalMode()
    {

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        if (firePoint != null)
        {
            // Set the color of the Gizmo line
            Gizmos.color = Color.green;

            // Draw a line from the firePoint in the forward direction
            Gizmos.DrawRay(firePoint.position, firePoint.forward * 5f);
        }
    }
}
