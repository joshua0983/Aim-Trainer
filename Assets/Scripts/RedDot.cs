using UnityEngine;

public class RedDot : MonoBehaviour
{
    private RedDotSpawner spawner;
    private float respawnDelay;

    public void Initialize(RedDotSpawner spawner, float respawnDelay)
    {
        this.spawner = spawner;
        this.respawnDelay = respawnDelay;
    }

    public void HandleClick()
    {
        Debug.Log("Red dot clicked!");
        spawner.RespawnRedDot(respawnDelay);
        Destroy(gameObject);
    }

    // Keep OnMouseDown for direct mouse clicks
    void OnMouseDown()
    {
        HandleClick();
    }
}