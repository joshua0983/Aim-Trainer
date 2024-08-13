using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3; // The maximum health of the enemy
    private int currentHealth;
    private EnemySpawner spawner;

    [SerializeField]
    private float _strafeSpeed = 3f; // Match this to the RedDot strafeSpeed
    public float strafeSpeed
    {
        get { return _strafeSpeed; }
        set
        {
            Debug.Log($"strafeSpeed changed from {_strafeSpeed} to {value}");
            _strafeSpeed = value;
        }
    }

    public float strafeDistance = 30f; // Maximum distance the enemy can move from its original position
    private float raycastBufferDistance = 0.5f; // Small buffer distance before reversing direction

    private Vector3 strafeDirection;
    private Vector3 initialPosition;
    private Rigidbody rb;

    public Vector3 safeZoneCenter;
    public float safeZoneRadius = 5f; // Radius of the safe zone around the player

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position; // Store the initial position for distance calculations
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        if (GameModeSelectionScript.StrafeEnabled)
        {
            // Initialize strafe direction to a random direction
            strafeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            StartCoroutine(Strafe());
        }

        // Set the player's position as the center of the safe zone
        safeZoneCenter = FindObjectOfType<PlayerMotor>().transform.position;

        // Log initial setup values
        Debug.Log($"Enemy Initialized with strafeSpeed: {strafeSpeed}, strafeDirection: {strafeDirection}, initialPosition: {initialPosition}, safeZoneCenter: {safeZoneCenter}");
    }

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    private void FixedUpdate()
    {
        if (GameModeSelectionScript.StrafeEnabled && rb != null)
        {
            // Adjusted Raycast distance to include a buffer
            float raycastDistance = strafeSpeed * Time.fixedDeltaTime + raycastBufferDistance;

            // Check for collisions using Raycast
            if (Physics.Raycast(rb.position, strafeDirection, out RaycastHit hitInfo, raycastDistance))
            {
                // If a collision is detected, reverse the direction
                strafeDirection = -strafeDirection;
                Debug.Log($"Collision detected with {hitInfo.collider.name}, reversing direction. New strafeDirection: {strafeDirection}");
            }
            else
            {
                // Calculate the new position
                Vector3 newPosition = rb.position + strafeDirection * strafeSpeed * Time.fixedDeltaTime;

                // Log movement details
                Debug.Log($"FixedUpdate - newPosition: {newPosition}, rb.position: {rb.position}, strafeDirection: {strafeDirection}, Time.fixedDeltaTime: {Time.fixedDeltaTime}, strafeSpeed: {strafeSpeed}");

                // Check if the new position would go outside the floor boundaries or enter the safe zone
                if (newPosition.x <= spawner.floorMin.x + raycastBufferDistance || newPosition.x >= spawner.floorMax.x - raycastBufferDistance ||
                    newPosition.z <= spawner.floorMin.z + raycastBufferDistance || newPosition.z >= spawner.floorMax.z - raycastBufferDistance ||
                    Vector3.Distance(newPosition, safeZoneCenter) <= safeZoneRadius)
                {
                    // Reverse direction if hitting the floor boundaries or entering the safe zone
                    strafeDirection = -strafeDirection;
                    Debug.Log($"Reversing Direction - new strafeDirection: {strafeDirection}");
                }
                else
                {
                    rb.MovePosition(newPosition);
                }
            }
        }
    }

    private IEnumerator Strafe()
    {
        while (true)
        {
            // Pick a direction and duration for the strafe
            strafeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            float strafeDuration = Random.Range(0.5f, 1f); // Random duration between 0.5 and 1 second

            // Log strafe direction and duration
            Debug.Log($"Strafe Coroutine - strafeDirection: {strafeDirection}, strafeDuration: {strafeDuration}");

            float elapsedTime = 0f;

            while (elapsedTime < strafeDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null; // Continue this every frame
            }
        }
    }

    public void HandleHit(bool isHeadshot)
    {
        if (isHeadshot)
        {
            currentHealth = 0; 
        }
        else
        {
            currentHealth--;
        }
        
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
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