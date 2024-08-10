using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform floor;
    public int maxEnemies = 5;

    private Vector3 floorMin;
    private Vector3 floorMax;
    private int currentEnemyCount = 0;

    // Add a difficulty-based delay before spawning a new enemy
    private float spawnDelay;

    void Start()
    {
        MeshRenderer floorRenderer = floor.GetComponent<MeshRenderer>();
        if (floorRenderer != null)
        {
            floorMin = floorRenderer.bounds.min;
            floorMax = floorRenderer.bounds.max;

            SetDifficulty(); // Set the spawn delay based on difficulty

            // Spawn initial enemies
            for (int i = 0; i < maxEnemies; i++)
            {
                SpawnEnemy();
            }
        }
        else
        {
            Debug.LogError("Floor does not have a MeshRenderer component");
        }
    }

    void SetDifficulty()
    {
        switch (DifficultyScript.difficulty)
        {
            case "Easy":
                spawnDelay = 2f; // Longer delay for easy mode
                break;
            case "Medium":
                spawnDelay = 1f; // Medium delay for medium mode
                break;
            case "Hard":
                spawnDelay = 0.5f; // Short delay for hard mode
                break;
            default:
                spawnDelay = 1f; // Default to medium if difficulty is not set
                break;
        }
    }

    void SpawnEnemy()
{
    if (currentEnemyCount >= maxEnemies) return;

    float minDistanceFromCenter = 8f; // Increase this value as needed for distance from the center
    Vector3 playerPosition = FindObjectOfType<PlayerMotor>().transform.position;

    Vector3 randomPosition;

    // Determine spawn along the x-axis and z-axis
    float xRange = Mathf.Clamp(playerPosition.x + Random.Range(minDistanceFromCenter, minDistanceFromCenter + 2f), floorMin.x + 1f, floorMax.x - 1f);
    float zRange = Mathf.Clamp(playerPosition.z + Random.Range(minDistanceFromCenter, minDistanceFromCenter + 2f), floorMin.z + 1f, floorMax.z - 1f);

    // Randomly decide whether to spawn along the x or z axis
    if (Random.value > 0.5f)
    {
        randomPosition = new Vector3(
            xRange,
            floor.position.y + 1f, // Adjust Y position to be on the floor
            Random.Range(floorMin.z + 1f, floorMax.z - 1f)
        );
    }
    else
    {
        randomPosition = new Vector3(
            Random.Range(floorMin.x + 1f, floorMax.x - 1f),
            floor.position.y + 1f,
            zRange
        );
    }

    GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    Enemy enemyComponent = enemy.GetComponent<Enemy>();
    if (enemyComponent != null)
    {
        enemyComponent.Initialize(this);
        currentEnemyCount++;
    }
    else
    {
        Debug.LogError("Enemy prefab does not have an Enemy component");
    }
}

    public void EnemyDefeated()
    {
        currentEnemyCount--;
        StartCoroutine(SpawnEnemyAfterDelay()); // Spawn a new enemy with a delay based on difficulty
    }

    private IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemy();
    }
}