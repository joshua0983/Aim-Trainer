using UnityEngine;

public class RedDot : MonoBehaviour
{
    private RedDotSpawner spawner;
    private float respawnDelay;
    public static string difficulty;

    public void Initialize(RedDotSpawner spawner, float respawnDelay)
    {
        Debug.Log($"RedDot initialized with spawner: {spawner.name}");
        this.spawner = spawner;
        this.respawnDelay = respawnDelay;
    }

    public void HandleClick()
    {
        Debug.Log("Red dot clicked!");
        spawner.RespawnRedDot(respawnDelay);
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown called");
        HandleClick();
    }
}