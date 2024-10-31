using UnityEngine;

public class enemy_bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f; // Bullet will destroy itself after this time
    public int damage = 10; // The damage the bullet can deal
    private Vector3 direction; // The direction the bullet will move

    private void Start()
    {
        // Automatically destroy the bullet after 'lifeTime' seconds
        Destroy(gameObject, lifeTime);

        // Calculate the direction of the bullet
        // Use the direction passed during instantiation
        direction = transform.forward; // Assuming the bullet is instantiated facing the target
    }

    private void Update()
    {
        // Apply movement using the calculated direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Lock the rotation around X and Z axes using quaternion
        Quaternion lockedRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = lockedRotation;

        // Ensure the enemy stays at the same Y position
        Vector3 position = transform.position;
        position.y = 0.9331f; // Replace with the desired Y value
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the bullet hits something, destroy it
        Destroy(gameObject);
    }
}
