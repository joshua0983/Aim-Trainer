using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private EnemySpawner spawner;

    [SerializeField]
    private float _strafeSpeed = 3f;
    public float strafeSpeed
    {
        get { return _strafeSpeed; }
        set { _strafeSpeed = value; }
    }

    public float strafeDistance = 30f;
    private float raycastBufferDistance = 0.5f;

    private Vector3 strafeDirection;
    private Vector3 initialPosition;
    private Rigidbody rb;

    public Vector3 safeZoneCenter;
    public float safeZoneRadius = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        if (GameModeSelectionScript.StrafeEnabled)
        {
            strafeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            StartCoroutine(Strafe());
        }

        safeZoneCenter = FindObjectOfType<PlayerMotor>().transform.position;
    }

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    private void FixedUpdate()
    {
        if (GameModeSelectionScript.StrafeEnabled && rb != null)
        {
            float raycastDistance = strafeSpeed * Time.fixedDeltaTime + raycastBufferDistance;

            if (Physics.Raycast(rb.position, strafeDirection, out RaycastHit hitInfo, raycastDistance))
            {
                strafeDirection = -strafeDirection;
            }
            else
            {
                Vector3 newPosition = rb.position + strafeDirection * strafeSpeed * Time.fixedDeltaTime;

                if (newPosition.x <= spawner.floorMin.x + raycastBufferDistance || newPosition.x >= spawner.floorMax.x - raycastBufferDistance ||
                    newPosition.z <= spawner.floorMin.z + raycastBufferDistance || newPosition.z >= spawner.floorMax.z - raycastBufferDistance ||
                    Vector3.Distance(newPosition, safeZoneCenter) <= safeZoneRadius)
                {
                    strafeDirection = -strafeDirection;
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
            strafeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            float strafeDuration = Random.Range(0.5f, 1f);

            float elapsedTime = 0f;

            while (elapsedTime < strafeDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
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
        ScoreManager.Instance?.AddScore(1);

        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}