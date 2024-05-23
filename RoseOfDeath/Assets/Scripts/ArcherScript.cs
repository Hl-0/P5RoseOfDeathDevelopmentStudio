using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f; // Movement speed of the archer
    public float projectileSpeed = 8f; // Speed of the projectile shot by the archer
    public GameObject projectilePrefab; // Prefab of the projectile

    private bool isDestroyed = false; // Flag to track if the archer has been destroyed

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
        if (!isDestroyed && player != null)
        {
            // Move the archer towards the player
            transform.LookAt(player.position);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void Shoot()
    {
        if (!isDestroyed && projectilePrefab != null && player != null)
        {
            // Calculate the direction to shoot the projectile
            Vector3 direction = player.position - transform.position;

            // Instantiate the projectile and set its velocity
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction.normalized * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }
        }
    }

    void OnDestroy()
    {
        isDestroyed = true; // Set the flag to indicate that the archer is destroyed
    }
}