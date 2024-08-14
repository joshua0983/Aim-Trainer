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
            playerVelocity.y = -2f;
        }
    }

    public void ProcessMovement(Vector2 input)
    {
        if (!isGrounded)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            if (playerVelocity.y < terminalVelocity)
            {
                playerVelocity.y = terminalVelocity;
            }
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }
}