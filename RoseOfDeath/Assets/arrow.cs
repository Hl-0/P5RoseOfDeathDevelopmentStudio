using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float arrowSpeed = 10f; // Speed of the arrow

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Launch();
    }

    void Launch()
    {
        if (player != null)
        {
            // Calculate direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Apply force to propel the arrow towards the player
            rb.velocity = direction * arrowSpeed;
        }
        else
        {
            Debug.LogWarning("Player reference not set in ArrowProjectile.");
        }
    }
}