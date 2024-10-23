using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyTime = 5f; // Time before the bullet is destroyed automatically

    void Start()
    {
        // Destroy the bullet after some time to prevent it from existing indefinitely
        Destroy(gameObject, destroyTime);
    }

    // Detect collision with an enemy
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the Enemy script
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Call the Die method on the enemy
                enemy.Die();
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
