using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 

public GameObject enemyPrefab; // Prefab of the first type of enemy
public GameObject secondEnemyPrefab; // Prefab of the second type of enemy
public Transform player; // Reference to the player's transform
public float spawnInterval = 3f; // Time interval between enemy spawns
public float spawnDistance = 20f; // Maximum distance from the player for spawning the first type of enemy
public float secondEnemyDistance = 10f; // Distance at which the second type of enemy spawns
public int touchLimit = 3; // Number of times the enemies can be touched before disappearing
public Vector3 minSpawnArea; // Minimum corner of the spawn area box
public Vector3 maxSpawnArea; // Maximum corner of the spawn area box
public float moveSpeed = 3f; // Movement speed of the enemies
public float projectileSpeed = 8f; // Speed of the projectile shot by the second type of enemy
public GameObject projectilePrefab; // Prefab of the projectile

private int enemyCount = 0; // Counter for the number of enemies spawned
private int enemiesKilled = 0; // Counter for the number of enemies killed
private bool isDestroyed = false; // Flag to track if the spawner is destroyed

private void Start()
{
    // Start spawning enemies periodically
    InvokeRepeating("SpawnEnemies", spawnInterval, spawnInterval);
}

private void SpawnEnemies()
{
    if (!isDestroyed)
    {
        // Spawn the first type of enemy
        SpawnEnemy(enemyPrefab, spawnDistance);

        // Spawn the second type of enemy
        SpawnEnemy(secondEnemyPrefab, secondEnemyDistance);
    }
}

private void SpawnEnemy(GameObject enemyPrefab, float distance)
{
    // Instantiate the enemy prefab at a random position within the spawn area
    Vector3 spawnPosition = GetRandomPosition();
    GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

    // Set the player reference, touch limit, move speed, and shooting behavior for the enemy
    Enemy enemyScript = enemyObject.GetComponent<Enemy>();
    if (enemyScript != null)
    {
        enemyScript.player = player;
        enemyScript.touchLimit = touchLimit;
        enemyScript.moveSpeed = moveSpeed;

        // If it's the second type of enemy, enable shooting behavior
        if (enemyPrefab == secondEnemyPrefab)
        {
            enemyScript.projectileSpeed = projectileSpeed;
            enemyScript.projectilePrefab = projectilePrefab;
        }

        enemyScript.OnEnemyDeath += HandleEnemyDeath; // Subscribe to the enemy's death event
        enemyCount++; // Increment the counter for spawned enemies
    }
    else
    {
        Debug.LogWarning("Enemy prefab does not have an Enemy script.");
    }
}

private void HandleEnemyDeath(Enemy enemy)
{
    enemy.OnEnemyDeath -= HandleEnemyDeath; // Unsubscribe from the enemy's death event
    enemyCount--; // Decrement the counter for spawned enemies
    enemiesKilled++; // Increment the counter for killed enemies
    Debug.Log("Enemies killed: " + enemiesKilled); // Log the number of enemies killed to the console
}

private Vector3 GetRandomPosition()
{
    // Generate a random position within the spawn area box
    float x = Random.Range(minSpawnArea.x, maxSpawnArea.x);
    float z = Random.Range(minSpawnArea.z, maxSpawnArea.z);
    float y = transform.position.y; // Keep the position on the same level as the spawner
    return new Vector3(x, y, z);
}

private void OnDestroy()
{
    isDestroyed = true; // Set the flag to indicate that the spawner is destroyed
}
} 