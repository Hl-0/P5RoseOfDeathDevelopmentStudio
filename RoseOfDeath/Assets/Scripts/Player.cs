using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public int moveSpeed;
    public int maxHealth = 100; // Maximum health of the player
    [SerializeField] private int currentHealth; // Current health of the player
    public float turnSpeed = 50f;
    public GameObject projectilePrefab; // Prefab of the projectile
    public Transform projectileSpawnPoint; // Spawn point for the projectile
    public float projectileSpeed = 10f; // Speed of the projectile shot by the player

    private bool doubleSpeedActive = false; // Flag to track if double speed is active
    private bool doubleSpeedReady = true; // Flag to track if double speed is ready to be activated again
    private float doubleSpeedDuration = 3f; // Duration of double speed
    private float doubleSpeedCooldown = 10f; // Cooldown period for double speed activation
    private float lastDoubleSpeedReadyTime; // Timestamp when double speed became ready

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health at the start
    }

    void Update()
    {
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.DownArrow))
                transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
        // Get input from arrow keys or WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the player
        if (doubleSpeedActive)
        {
            // Double speed if double speed is active
            transform.Translate(moveDirection * moveSpeed * 2 * Time.deltaTime);
        }
        else
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Shooting logic
        if (Input.GetKeyDown(KeyCode.V) && !doubleSpeedActive) // Only allow shooting if double speed is not active
        {
            Shoot();
        }

        // Activate double speed if 'F' key is pressed and it's ready
        if (Input.GetKeyDown(KeyCode.F) && doubleSpeedReady)
        {
            ActivateDoubleSpeed();
        }

        // Check if double speed can be activated again
        if (Time.time - lastDoubleSpeedReadyTime >= doubleSpeedCooldown && !doubleSpeedReady)
        {
            doubleSpeedReady = true; // Enable double speed
            Debug.Log("Double speed is available again. Press 'F' to activate.");
        }
    }

    // Method to reduce player's health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the specified amount

        // Check if player's health drops to zero or below
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method if player's health is zero or below
        }
    }

    // Method to handle player's death
    void Die()
    {
        // Perform actions to handle player's death, such as game over screen, respawn, etc.
        Debug.Log("Player has died!");
        // You can add more actions here, like reloading the scene or showing a game over screen
    }

    // Method to shoot a projectile
    void Shoot()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate the projectile prefab at the spawn point
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Get the Rigidbody component of the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set the velocity of the projectile to move in the forward direction of the spawn point
                rb.velocity = projectileSpawnPoint.forward * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }

            // Destroy the projectile after 4 seconds
            Destroy(projectile, 4f);
        }
        else
        {
            Debug.LogWarning("Projectile prefab or projectile spawn point is not assigned.");
        }
    }

    // Method to activate double speed
    void ActivateDoubleSpeed()
    {
        doubleSpeedActive = true; // Activate double speed
        doubleSpeedReady = false; // Disable double speed activation until cooldown ends
        lastDoubleSpeedReadyTime = Time.time; // Record the time when double speed became unavailable

        Debug.Log("Double speed activated!");

        // Schedule deactivation of double speed after duration
        Invoke("DeactivateDoubleSpeed", doubleSpeedDuration);
    }

    // Method to deactivate double speed
    void DeactivateDoubleSpeed()
    {
        doubleSpeedActive = false; // Deactivate double speed
        Debug.Log("Double speed deactivated.");
    }
}