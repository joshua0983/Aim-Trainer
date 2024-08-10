using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3; // The maximum health of the enemy
    private int currentHealth;
    private EnemySpawner spawner;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void HandleHit(bool isHeadshot)
{
    Debug.Log($"HandleHit called. Is headshot: {isHeadshot}"); // Log at the start

    if (isHeadshot)
    {
        Debug.Log("Headshot detected! Setting health to 0.");
        currentHealth = 0; // Instantly kill the enemy
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
        
        // Check if the spawner is not null to avoid null reference exception
        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }
        else
        {
            Debug.LogError("Spawner reference is missing!");
        }
        
        Destroy(gameObject); // Destroy the entire enemy GameObject
    }
}