using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static bool ShouldStartPaused = false; // Added static variable

    public PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private PauseScript pauseScript;

    private string[] gameScenes = { "Flicking", "EnemyShooting" };

void Awake()
{
    // Initialize the PlayerInput and assign the OnFoot actions
    playerInput = new PlayerInput();
    onFoot = playerInput.OnFoot;

    // Assign components from the same GameObject
    motor = GetComponent<PlayerMotor>();
    look = GetComponent<PlayerLook>();
    pauseScript = FindObjectOfType<PauseScript>();  // This finds any PauseScript in the scene

    // Debug logs to ensure components are assigned
    if (motor == null)
    {
        Debug.LogError("PlayerMotor component is not found on this GameObject.");
    }

    if (look == null)
    {
        Debug.LogError("PlayerLook component is not found on this GameObject.");
    }

    if (pauseScript == null)
    {
        Debug.LogError("PauseScript component is not found in the scene.");
    }

    // Check playerInput initialization
    if (playerInput != null)
    {
        onFoot.Shoot.performed += ctx => Shoot();
    }
    else
    {
        Debug.LogError("PlayerInput is not initialized.");
    }
}
    void Start()
    {
        if (IsGameScene())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (ShouldStartPaused && pauseScript != null)
            {
                pauseScript.PauseGame();
                ShouldStartPaused = false; // Reset the flag after pausing
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && IsGameScene())
        {
            if (pauseScript != null)
            {
                if (pauseScript.IsPaused) pauseScript.ContinueGame();
                else pauseScript.PauseGame();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }

    void FixedUpdate()
    {
        if (motor != null)
        {
            motor.ProcessMovement(onFoot.Movement.ReadValue<Vector2>());
        }
    }

void LateUpdate()
{
    if (look == null)
    {
        Debug.LogError("PlayerLook component is not assigned or found!");
        return;  // Exit the method to avoid the null reference
    }

    // No need to check for onFoot being null; instead, ensure playerInput is initialized
    if (playerInput == null)
    {
        Debug.LogError("PlayerInput is not initialized!");
        return;  // Exit the method to avoid the null reference
    }

    look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
}

    private void OnEnable()
    {
        if (playerInput != null) onFoot.Enable();
    }

    private void OnDisable()
    {
        if (playerInput != null) onFoot.Disable();
    }

    private void Shoot()
{
    if (look == null || look.cam == null) return;

    Ray ray = look.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        Debug.Log($"Raycast hit: {hit.collider.name}, Tag: {hit.collider.tag}");

        // Check if we hit the EnemyHead or EnemyBody
        Enemy enemy = hit.collider.GetComponentInParent<Enemy>(); // Use GetComponentInParent
        if (enemy != null)
        {
            bool isHeadshot = hit.collider.CompareTag("EnemyHead");
            Debug.Log(isHeadshot ? "Headshot detected!" : "Body hit detected!");
            Debug.Log("Calling HandleHit on Enemy.");
            enemy.HandleHit(isHeadshot); // Call the function
        }
        else
        {
            Debug.LogWarning("Enemy script not found on the hit object or its parent.");
        }
    }
    else
    {
        Debug.Log("Raycast did not hit anything.");
    }
}
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private bool IsGameScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        foreach (string sceneName in gameScenes)
        {
            if (currentSceneName == sceneName) return true;
        }
        return false;
    }

    public void TriggerPause()
    {
        if (pauseScript != null)
        {
            if (pauseScript.IsPaused) pauseScript.ContinueGame();
            else pauseScript.PauseGame();
        }
    }
}