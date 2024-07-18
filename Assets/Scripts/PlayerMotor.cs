using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    public bool isGrounded;
    public float gravityValue = -9.81f;
    public float terminalVelocity = -20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Smoothly reduce downward velocity
        }
    }

    public void ProcessMovement(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        // Apply gravity if not grounded
        if (!isGrounded)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            // Clamp to terminal velocity
            if (playerVelocity.y < terminalVelocity)
            {
                playerVelocity.y = terminalVelocity;
            }
        }

        // Apply vertical velocity
        controller.Move(playerVelocity * Time.deltaTime);

        // Conditional log to reduce frequency
        if (Mathf.Abs(playerVelocity.y) > 0.01f)
        {
            // Debug.Log("Player Velocity: " + playerVelocity.y);
        }
    }
}