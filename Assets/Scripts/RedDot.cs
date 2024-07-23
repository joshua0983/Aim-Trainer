using UnityEngine;
using UnityEngine.InputSystem;

public class RedDot : MonoBehaviour
{
    private RedDotSpawner spawner;
    private float respawnDelay;
    public static string difficulty;

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

    void OnMouseDown()
    {
        HandleClick();
    }
}