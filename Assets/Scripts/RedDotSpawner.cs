using System.Collections;
using UnityEngine;

public class RedDotSpawner : MonoBehaviour
{
    public GameObject redDotPrefab;
    public Transform wall;
    private float spawnDelay = 0.5f;
    private float disappearDelay;

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
    }

    void SetDifficulty()
    {
        switch (DifficultyScript.difficulty)
        {
            case "Easy":
                disappearDelay = float.MaxValue;
                break;
            case "Medium":
                disappearDelay = 1.25f;
                break;
            case "Hard":
                disappearDelay = 0.75f;
                break;
            default:
                disappearDelay = 1.25f;
                break;
        }
    }

    void SpawnRedDot()
    {
        if (redDotPrefab == null || wall == null || isSpawning)
        {
            return;
        }

        isSpawning = true;

        float xRangeMin = wallMin.x + (wallMax.x - wallMin.x) * 0.2f;
        float xRangeMax = wallMax.x - (wallMax.x - wallMin.x) * 0.2f;
        float yRangeMin = wallMin.y + (wallMax.y - wallMin.y) * 0.1f;
        float yRangeMax = wallMax.y - (wallMax.y - wallMin.y) * 0.1f;

        Vector3 randomPosition = new Vector3(
            Random.Range(xRangeMin, xRangeMax),
            Random.Range(yRangeMin, yRangeMax),
            wall.position.z - 1.5f
        );

        GameObject redDot = Instantiate(redDotPrefab, randomPosition, Quaternion.identity);
        RedDot redDotComponent = redDot.GetComponent<RedDot>();
        if (redDotComponent != null)
        {
            redDotComponent.Initialize(this, spawnDelay);

            if (disappearDelay < float.MaxValue)
            {
                StartCoroutine(DestroyAndRespawn(redDot, disappearDelay));
            }
        }
        else
        {
            isSpawning = false;
        }
    }

    private IEnumerator DestroyAndRespawn(GameObject redDot, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (redDot != null)
        {
            Destroy(redDot);
            isSpawning = false;
            SpawnRedDot();
        }
    }

    public void RespawnRedDot(float delay)
    {
        StartCoroutine(RespawnRedDotCoroutine(spawnDelay));
    }

    private IEnumerator RespawnRedDotCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = false;
        SpawnRedDot();
    }
}