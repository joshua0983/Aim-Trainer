using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        // Check if the required components are assigned
        if (motor == null)
        {
            Debug.LogError("PlayerMotor component is missing");
        }

        if (look == null)
        {
            Debug.LogError("PlayerLook component is missing");
        }
        else if (look.cam == null)
        {
            Debug.LogError("PlayerLook's Camera (cam) is not assigned");
        }

        // Bind the shooting action
        onFoot.Shoot.performed += ctx => Shoot();
    }

    void Start()
    {
        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check for the Escape key press to unlock the cursor and exit the game
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            #if UNITY_EDITOR
            // Stop play mode in the editor
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            // Quit the application
            Application.Quit();
            #endif
        }
    }

    void FixedUpdate()
    {
        if (motor == null)
        {
            Debug.LogError("PlayerMotor component is missing in FixedUpdate");
            return;
        }
        motor.ProcessMovement(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        if (look == null)
        {
            Debug.LogError("PlayerLook component is missing in LateUpdate");
            return;
        }
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    private void Shoot()
    {
        if (look == null || look.cam == null)
        {
            Debug.LogError("PlayerLook or Player camera is not assigned in Shoot");
            return;
        }

        Debug.Log("Shoot action performed");

        // Use the center of the screen for raycasting
        Ray ray = look.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast hit detected");

            RedDot redDot = hit.collider.GetComponent<RedDot>();
            if (redDot != null)
            {
                Debug.Log("Red dot hit by raycast!");
                redDot.HandleClick(); // Call the public method
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // Lock and hide the cursor when the game window is focused
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Unlock and show the cursor when the game window is unfocused
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}