using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour

{
   
    public GameObject enemyPrefab; // Prefab of the enemy GameObject
    public Transform player; // Reference to the player's transform
    public float spawnInterval = 3f; // Time interval between enemy spawns
    public int touchLimit = 3; // Number of times the enemy can be touched before disappearing
    public Vector3 minSpawnArea; // Minimum corner of the spawn area box
    public Vector3 maxSpawnArea; // Maximum corner of the spawn area box

    private int enemyCount = 0; // Counter for the number of enemies spawned
    private int enemiesKilled = 0; // Counter for the number of enemies killed
    private bool isDestroyed = false; // Flag to track if the spawner is destroyed

    void Start()
    {
        // Start spawning enemies periodically
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (!isDestroyed)
        {
            // Instantiate the enemy prefab at a random position within the spawn area
            Vector3 spawnPosition = GetRandomPosition();
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Set the player reference and touch limit for the enemy script
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.player = player;
                enemyScript.touchLimit = touchLimit;
                enemyScript.OnEnemyDeath += HandleEnemyDeath; // Subscribe to the enemy's death event
                enemyCount++; // Increment the counter for spawned enemies
            }
            else
            {
                Debug.LogWarning("Enemy prefab does not have an Enemy script.");
            }
        }
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnEnemyDeath -= HandleEnemyDeath; // Unsubscribe from the enemy's death event
        enemyCount--; // Decrement the counter for spawned enemies
        enemiesKilled++; // Increment the counter for killed enemies
        Debug.Log("Enemies killed: " + enemiesKilled); // Log the number of enemies killed to the console
    }

    Vector3 GetRandomPosition()
    {
        // Generate a random position within the spawn area box
        float x = Random.Range(minSpawnArea.x, maxSpawnArea.x);
        float z = Random.Range(minSpawnArea.z, maxSpawnArea.z);
        float y = transform.position.y; // Keep the position on the same level as the spawner
        return new Vector3(x, y, z);
    }

    void OnDestroy()
    {
        isDestroyed = true; // Set the flag to indicate that the spawner is destroyed
    }
}