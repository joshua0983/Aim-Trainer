using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3; // The maximum health of the enemy
    private int currentHealth;
    private EnemySpawner spawner;

    public float strafeSpeed = 0.01f;
    public float strafeDistance = 30f; 

    private Vector3 strafeDirection;
    private Vector3 initialPosition;

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position; // Store the initial position for distance calculations
        
        if (GameModeSelectionScript.StrafeEnabled)
        {
            // Initialize strafe direction to a random direction
            strafeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            StartCoroutine(Strafe());
        }
    }

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

   private IEnumerator Strafe()
{
    while (true)
    {
        // Pick a direction and duration for the strafe
        strafeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        float strafeDuration = Random.Range(0.5f, 1f); // Random duration between 0.5 and 1 second

        float elapsedTime = 0f;

        while (elapsedTime < strafeDuration)
        {
            // Move the enemy in the strafe direction
            Vector3 newPosition = transform.position + strafeDirection * strafeSpeed * Time.deltaTime;

            // Check if the new position would go outside the floor boundaries
            if (newPosition.x <= spawner.floorMin.x || newPosition.x >= spawner.floorMax.x ||
                newPosition.z <= spawner.floorMin.z || newPosition.z >= spawner.floorMax.z)
            {
                // Reverse direction if hitting the floor boundaries
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

    public void HandleHit(bool isHeadshot)
    {
        Debug.Log($"HandleHit called. Is headshot: {isHeadshot}"); 

        if (isHeadshot)
        {
            Debug.Log("Headshot detected! Setting health to 0.");
            currentHealth = 0; 
        }
        else
        {
            currentHealth--;
            Debug.Log("Body hit detected! Current health: " + currentHealth);
        }

        Debug.Log($"Health after hit: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Debug.Log("Health is 0 or less, destroying enemy.");
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Debug.Log("Destroying enemy now.");
        
        // Add score
        ScoreManager.Instance?.AddScore(1); 
        
        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }
        else
        {
            Debug.LogError("Spawner reference is missing!");
        }
        
        Destroy(gameObject); 
    }
}