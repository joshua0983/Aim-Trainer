using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform floor;
    public int maxEnemies;

    public Vector3 floorMin;
    public Vector3 floorMax;
    private int currentEnemyCount = 0;

    private float spawnDelay = 0.5f;

    void Start()
    {
        SetDifficulty();

        MeshRenderer floorRenderer = floor.GetComponent<MeshRenderer>();
        if (floorRenderer != null)
        {
            floorMin = floorRenderer.bounds.min;
            floorMax = floorRenderer.bounds.max;

            for (int i = 0; i < maxEnemies; i++)
            {
                SpawnEnemy();
            }
        }
    }

    void SetDifficulty()
    {
        switch (DifficultyScript.difficulty)
        {
            case "Easy":
                maxEnemies = 7;
                break;
            case "Medium":
                maxEnemies = 5;
                break;
            case "Hard":
                maxEnemies = 3;
                break;
            default:
                maxEnemies = 7;
                break;
        }
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Vector3 randomPosition = GetRandomPosition();

        GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.Initialize(this);
            currentEnemyCount++;
        }
    }

    Vector3 GetRandomPosition()
    {
        float minDistanceFromCenter = 8f;
        Vector3 playerPosition = FindObjectOfType<PlayerMotor>().transform.position;

        float xRange = Mathf.Clamp(playerPosition.x + Random.Range(minDistanceFromCenter, minDistanceFromCenter + 2f), floorMin.x + 1f, floorMax.x - 1f);
        float zRange = Mathf.Clamp(playerPosition.z + Random.Range(minDistanceFromCenter, minDistanceFromCenter + 2f), floorMin.z + 1f, floorMax.z - 1f);

        if (Random.value > 0.5f)
        {
            return new Vector3(xRange, floor.position.y + 1f, Random.Range(floorMin.z + 1f, floorMax.z - 1f));
        }
        else
        {
            return new Vector3(Random.Range(floorMin.x + 1f, floorMax.x - 1f), floor.position.y + 1f, zRange);
        }
    }

    public void EnemyDefeated()
    {
        currentEnemyCount--;
        StartCoroutine(SpawnEnemyAfterDelay());
    }

    private IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemy();
    }
}