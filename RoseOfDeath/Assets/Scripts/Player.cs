using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public int moveSpeed;
    public int maxHealth = 100; // Maximum health of the player
    private int currentHealth; // Current health of the player

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health at the start
    }

    void Update()
    {
        // Get input from arrow keys or WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the player
        transform.Translate(moveDirection * moveSpeed * 5 * Time.deltaTime);
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
}