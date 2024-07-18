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
        if (redDotPrefab == null || wall == null)
        {
            Debug.LogError("Cannot spawn RedDot because redDotPrefab or wall is not assigned");
            return;
        }

        Vector3 randomPosition = new Vector3(
            Random.Range(wallMin.x, wallMax.x),
            Random.Range(wallMin.y, wallMax.y),
            wall.position.z
        );

        GameObject redDot = Instantiate(redDotPrefab, randomPosition, Quaternion.identity);
        RedDot redDotComponent = redDot.GetComponent<RedDot>();
        if (redDotComponent != null)
        {
            redDotComponent.Initialize(this, spawnDelay);
        }
        else
        {
            Debug.LogError("RedDotPrefab does not have a RedDot component");
        }
    }

    public void RespawnRedDot(float delay)
    {
        StartCoroutine(RespawnRedDotCoroutine(delay));
    }

    private IEnumerator RespawnRedDotCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnRedDot();
    }
}