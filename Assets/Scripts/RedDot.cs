using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RedDot : MonoBehaviour
{
    private RedDotSpawner spawner;
    private float respawnDelay;

    public float strafeSpeed = 0.01f; 
    public float strafeDistance = 30f; // Maximum distance the red dot can move from its original position

    private Vector3 strafeDirection;
    private Vector3 initialPosition;

    public void Initialize(RedDotSpawner spawner, float respawnDelay)
    {
        this.spawner = spawner;
        this.respawnDelay = respawnDelay;
        initialPosition = transform.position; // Store the initial position for distance calculations

        if (GameModeSelectionScript.StrafeEnabled)
        {
            // Initialize strafe direction to a random direction
            strafeDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            StartCoroutine(Strafe());
        }
    }

private IEnumerator Strafe()
{
    while (true)
    {
        // Pick a direction and duration for the strafe
        strafeDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        float strafeDuration = Random.Range(0.5f, 1f); // Random duration between 0.5 and 1 second

        float elapsedTime = 0f;

        while (elapsedTime < strafeDuration)
        {
            // Move the red dot in the strafe direction
            Vector3 newPosition = transform.position + strafeDirection * strafeSpeed * Time.deltaTime;

            // Check if the new position would go outside the wall boundaries
            if (newPosition.x <= spawner.wallMin.x || newPosition.x >= spawner.wallMax.x ||
                newPosition.y <= spawner.wallMin.y || newPosition.y >= spawner.wallMax.y)
            {
                // Reverse direction if hitting the wall boundaries
                strafeDirection = -strafeDirection;
            }
            else
            {
                transform.Translate(strafeDirection * strafeSpeed * Time.deltaTime, Space.World);
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Continue this every frame
        }
    }
}

    public void HandleClick()
    {
        Debug.Log("Red dot clicked!");
        spawner.RespawnRedDot(respawnDelay);

        ScoreManager.Instance?.AddScore(1);

        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        HandleClick();
    }
}