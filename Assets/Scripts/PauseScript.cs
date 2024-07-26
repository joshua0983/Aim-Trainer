using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public Button ContinueButton;
    public Button OptionsButton;
    public Button ExitToMenuButton;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI; // Reference to duplicated options menu

    public bool IsPaused { get; private set; } = false;

    void Start()
    {
        ContinueButton.onClick.AddListener(ContinueGame);
        OptionsButton.onClick.AddListener(OpenOptions);
        ExitToMenuButton.onClick.AddListener(ExitGame);
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false); // Ensure it is inactive by default
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsPaused = true;
    }

    public void ContinueGame()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false); // Ensure options menu is inactive
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsPaused = false;
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true); // Activate the options menu
        OptionsMenuController.CameFromGame = true; // Indicate we came from the game
        
        // Set the options and pause menu references in OptionsMenuController
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