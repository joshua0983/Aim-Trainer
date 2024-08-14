using System.Collections;
using UnityEngine;

public class RedDot : MonoBehaviour
{
    private RedDotSpawner spawner;
    private float respawnDelay;
    public float strafeSpeed = 0.01f;
    public float strafeDistance = 30f;

    private Vector3 strafeDirection;
    private Vector3 initialPosition;

    public void Initialize(RedDotSpawner spawner, float respawnDelay)
    {
        this.spawner = spawner;
        this.respawnDelay = respawnDelay;
        initialPosition = transform.position;

        if (GameModeSelectionScript.StrafeEnabled)
        {
            strafeDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            StartCoroutine(Strafe());
        }
    }

    private IEnumerator Strafe()
    {
        while (true)
        {
            strafeDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            float strafeDuration = Random.Range(0.5f, 1f);

            float elapsedTime = 0f;

            while (elapsedTime < strafeDuration)
            {
                Vector3 newPosition = transform.position + strafeDirection * strafeSpeed * Time.deltaTime;

                if (newPosition.x <= spawner.wallMin.x || newPosition.x >= spawner.wallMax.x ||
                    newPosition.y <= spawner.wallMin.y || newPosition.y >= spawner.wallMax.y)
                {
                    strafeDirection = -strafeDirection;
                }
                else
                {
                    transform.Translate(strafeDirection * strafeSpeed * Time.deltaTime, Space.World);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void HandleClick()
    {
        spawner.RespawnRedDot(respawnDelay);
        ScoreManager.Instance?.AddScore(1);
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        HandleClick();
    }
}