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

        Ray ray = look.cam.ScreenPointToRay(Mouse.current.position.ReadValue());
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
}