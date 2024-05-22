using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f; // Movement speed of the enemy
    public int touchLimit = 3; // Number of times the enemy can be touched before disappearing

    private int touchCount = 0; // Counter for the number of times the enemy has been touched
    private bool isDestroyed = false; // Flag to track if the enemy has been destroyed

    // Define a delegate for the enemy death event
    public delegate void EnemyDeathEvent(Enemy enemy);
    // Define the event
    public event EnemyDeathEvent OnEnemyDeath;

    void Update()
    {
        if (!isDestroyed)
        {
            if (touchCount < touchLimit)
            {
                if (player != null && PlayerIsInRange())
                {
                    // Calculate the direction from the enemy to the player
                    Vector3 direction = player.position - transform.position;
                    direction.y = 0f; // Keep the enemy grounded

                    // Normalize the direction to get a unit vector
                    direction.Normalize();

                    // Move the enemy towards the player
                    transform.Translate(direction * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Destroy the enemy if the touch count reaches the limit
                DestroyEnemy();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDestroyed && other.CompareTag("Player"))
        {
            // Increment the touch count when the player touches the enemy
            touchCount++;

            // Check if the touch count exceeds the limit
            if (touchCount >= touchLimit)
            {
                // Destroy the enemy if the touch count reaches the limit
                DestroyEnemy();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDestroyed && collision.gameObject.CompareTag("Player"))
        {
            // Increment the touch count when the player touches the enemy
            touchCount++;

            // Check if the touch count exceeds the limit
            if (touchCount >= touchLimit)
            {
                // Destroy the enemy if the touch count reaches the limit
                DestroyEnemy();
            }
        }
    }

    bool PlayerIsInRange()
    {
        return Vector3.Distance(transform.position, player.position) > 1.0f;
    }

    void DestroyEnemy()
    {
        // Invoke the enemy death event before destroying the enemy GameObject
        OnEnemyDeath?.Invoke(this);

        // Destroy the enemy GameObject
        Destroy(gameObject);
        isDestroyed = true;
    }
}