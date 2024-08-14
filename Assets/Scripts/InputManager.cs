using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static bool ShouldStartPaused = false;
    public PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private PauseScript pauseScript;
    private string[] gameScenes = { "Flicking", "EnemyShooting" };

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        pauseScript = FindObjectOfType<PauseScript>();

        if (playerInput != null)
        {
            onFoot.Shoot.performed += ctx => Shoot();
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
                ShouldStartPaused = false;
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
        if (look != null)
        {
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
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
            RedDot redDot = hit.collider.GetComponent<RedDot>();
            if (redDot != null)
            {
                redDot.HandleClick();
                return;
            }

            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                bool isHeadshot = hit.collider.CompareTag("EnemyHead");
                enemy.HandleHit(isHeadshot);
            }
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