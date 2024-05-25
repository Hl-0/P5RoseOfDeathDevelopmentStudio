using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f; // Movement speed of the enemy
    public int touchLimit = 2; // Number of times the enemy can be touched before disappearing
    public GameObject projectilePrefab; // Prefab of the projectile
    public float projectileSpeed = 8f; // Speed of the projectile shot by the enemy
    public float shootInterval = 1f; // Interval between shots

    private int touchCount = 0; // Counter for the number of times the enemy has been touched
    private bool isDestroyed = false; // Flag to track if the enemy has been destroyed
    private bool canShoot = true; // Flag to track if the enemy can shoot
    private float shootTimer = 0f; // Timer to control the shooting frequency

    // Define a delegate for the enemy death event
    public delegate void EnemyDeathEvent(Enemy enemy);
    // Define the event
    public event EnemyDeathEvent OnEnemyDeath;

    void Start()
    {
        // If the player reference is not set, try to find the player GameObject
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player GameObject not found!");
            }
        }
    }

    void Update()
    {
        if (!isDestroyed)
        {
            if (touchCount < touchLimit)
            {
                if (player != null && PlayerIsInRange())
                {
                    // Calculate the direction from the enemy to the player
                    Vector3 direction = (player.position - transform.position).normalized;

                    // Move the enemy towards the player
                    transform.Translate(direction * moveSpeed * Time.deltaTime);

                    // Update shoot timer
                    shootTimer += Time.deltaTime;
                    if (shootTimer >= shootInterval && canShoot)
                    {
                        // Shoot projectile towards player position
                        Shoot(direction);
                        // Reset shoot timer
                        shootTimer = 0f;
                    }
                }
            }
            else
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
            // Reduce player's health when colliding with the player
            collision.gameObject.GetComponent<Player>().TakeDamage(10);
            // Destroy the enemy when colliding with the player
            DestroyEnemy();
        }
        else if (!isDestroyed && collision.gameObject.CompareTag("PlayerProjectile"))
        {
            // Increment the touch count when hit by the player's projectile
            TakeDamage(1); // Player's projectile deals 1 damage
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

    void Shoot(Vector3 direction)
    {
        if (projectilePrefab != null && player != null)
        {
            // Calculate the direction towards the player
            Vector3 targetDirection = (player.position - transform.position).normalized;

            // Instantiate the projectile and set its position to the enemy's position
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Calculate the velocity vector for the projectile
            Vector3 velocity = targetDirection * projectileSpeed;

            // Get the Rigidbody component of the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set the velocity of the projectile to the calculated velocity
                rb.velocity = velocity;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }

            // Destroy the projectile after 2 seconds
            Destroy(projectile, 2f);
        }
    }

    // Method to make the enemy take damage
    public void TakeDamage(int damage)
    {
        touchCount += damage; // Increment the touch count by the specified amount

        // Check if the touch count exceeds the limit
        if (touchCount >= touchLimit)
        {
            // Destroy the enemy if the touch count reaches the limit
            DestroyEnemy();
        }
    }
}