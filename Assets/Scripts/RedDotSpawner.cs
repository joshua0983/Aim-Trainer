using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotSpawner : MonoBehaviour
{
    public GameObject redDotPrefab;
    public Transform wall;
    public float spawnDelay = 0.5f;

    private Vector3 wallMin;
    private Vector3 wallMax;
    private bool isSpawning = false;

    void Start()
    {
        if (redDotPrefab == null)
        {
            Debug.LogError("RedDotPrefab is not assigned in the Inspector");
        }

        if (wall == null)
        {
            Debug.LogError("Wall Transform is not assigned in the Inspector");
        }
        else
        {
            MeshRenderer wallRenderer = wall.GetComponent<MeshRenderer>();
            if (wallRenderer != null)
            {
                wallMin = wallRenderer.bounds.min;
                wallMax = wallRenderer.bounds.max;

                // Log the wall boundaries
                Debug.Log($"Wall Min: {wallMin}, Wall Max: {wallMax}");
            }
            else
            {
                Debug.LogError("Wall does not have a MeshRenderer component");
            }

            SpawnRedDot();
        }
    }

    void SpawnRedDot()
    {
        if (redDotPrefab == null || wall == null || isSpawning)
        {
            Debug.LogError("Cannot spawn RedDot because redDotPrefab or wall is not assigned or another red dot is already spawning");
            return;
        }

        isSpawning = true;

        // Adjust the boundaries to limit the spawn area
        float xRangeMin = wallMin.x + (wallMax.x - wallMin.x) * 0.2f; // 20% from the left
        float xRangeMax = wallMax.x - (wallMax.x - wallMin.x) * 0.2f; // 20% from the right
        float yRangeMin = wallMin.y + (wallMax.y - wallMin.y) * 0.1f; // 10% from the bottom
        float yRangeMax = wallMax.y - (wallMax.y - wallMin.y) * 0.1f; // 10% from the top

        Vector3 randomPosition = new Vector3(
            Random.Range(xRangeMin, xRangeMax),
            Random.Range(yRangeMin, yRangeMax),
            wall.position.z - 1.5f // Adjust this value as needed to move the red dot in front of the wall
        );

        Debug.Log($"Spawning Red Dot at: {randomPosition}");

        GameObject redDot = Instantiate(redDotPrefab, randomPosition, Quaternion.identity);
        RedDot redDotComponent = redDot.GetComponent<RedDot>();
        if (redDotComponent != null)
        {
            redDotComponent.Initialize(this, spawnDelay);
            Debug.Log("Red Dot spawned successfully");
        }
        else
        {
            Debug.LogError("RedDotPrefab does not have a RedDot component");
            isSpawning = false; // Ensure isSpawning is reset if there's an error
        }
    }

    public void RespawnRedDot(float delay)
    {
        StartCoroutine(RespawnRedDotCoroutine(delay));
    }

    private IEnumerator RespawnRedDotCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = false; // Allow spawning again after the delay
        SpawnRedDot();
    }
}