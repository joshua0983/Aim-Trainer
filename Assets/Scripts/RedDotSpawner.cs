using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotSpawner : MonoBehaviour
{
    public GameObject redDotPrefab;
    public Transform wall;
    private float spawnDelay = 0.5f; // Fixed respawn delay after hit for all difficulties
    private float disappearDelay; // Delay before disappearing, based on difficulty

    public Vector3 wallMin;
    public Vector3 wallMax;
    private bool isSpawning = false;

    void Start()
    {
        SetDifficulty();

        MeshRenderer wallRenderer = wall.GetComponent<MeshRenderer>();
        if (wallRenderer != null)
        {
            wallMin = wallRenderer.bounds.min;
            wallMax = wallRenderer.bounds.max;

            SpawnRedDot();
        }
        else
        {
            Debug.LogError("Wall does not have a MeshRenderer component");
        }
    }

    void SetDifficulty()
    {
        switch (DifficultyScript.difficulty)
        {
            case "Easy":
                disappearDelay = float.MaxValue; // Red dot never disappears automatically
                break;
            case "Medium":
                disappearDelay = 1.25f; // Red dot disappears after 1.5 seconds
                break;
            case "Hard":
                disappearDelay = 0.75f; // Red dot disappears after 1 second
                break;
            default:
                disappearDelay = 1.25f; // Default to medium if difficulty is not set
                break;
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

        GameObject redDot = Instantiate(redDotPrefab, randomPosition, Quaternion.identity);
        RedDot redDotComponent = redDot.GetComponent<RedDot>();
        if (redDotComponent != null)
        {
            redDotComponent.Initialize(this, spawnDelay); // Pass the spawnDelay for hit respawn
            Debug.Log("Red Dot spawned successfully");

            // Handle automatic disappearance based on difficulty
            if (disappearDelay < float.MaxValue)
            {
                StartCoroutine(DestroyAndRespawn(redDot, disappearDelay));
            }
        }
        else
        {
            Debug.LogError("RedDotPrefab does not have a RedDot component");
            isSpawning = false; // Ensure isSpawning is reset if there's an error
        }
    }

    private IEnumerator DestroyAndRespawn(GameObject redDot, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (redDot != null)
        {
            Destroy(redDot);
            isSpawning = false; // Allow spawning again after the delay
            SpawnRedDot();
        }
    }

    public void RespawnRedDot(float delay)
    {
        StartCoroutine(RespawnRedDotCoroutine(spawnDelay)); // Use the fixed respawn delay for hits
    }

    private IEnumerator RespawnRedDotCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = false; // Allow spawning again after the delay
        SpawnRedDot();
    }
}