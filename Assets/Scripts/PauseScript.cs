using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public Button ContinueButton;
    public Button OptionsButton;
    public Button ExitToMenuButton;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    public bool IsPaused { get; private set; } = false;

    void Start()
    {
        ContinueButton.onClick.AddListener(ContinueGame);
        OptionsButton.onClick.AddListener(OpenOptions);
        ExitToMenuButton.onClick.AddListener(ExitGame);
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsPaused = true;

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer != null)
        {
            gameTimer.PauseTimer();
        }
    }

    public void ContinueGame()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsPaused = false;

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer != null)
        {
            gameTimer.ResumeTimer();
        }
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        OptionsMenuController.CameFromGame = true;

        OptionsMenuController optionsController = optionsMenuUI.GetComponent<OptionsMenuController>();
        if (optionsController != null)
        {
            optionsController.optionsMenuUI = optionsMenuUI;
            optionsController.pauseMenuUI = pauseMenuUI;
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}