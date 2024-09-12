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
            // Destroy the enemy
            Destroy(collision.gameObject);

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
