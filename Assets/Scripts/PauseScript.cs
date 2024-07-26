using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{
    public Button ContinueButton;
    public Button OptionsButton;
    public Button ExitToMenuButton;
    public GameObject pauseMenuUI;

    public bool IsPaused { get; private set; } = false;

    void Start()
    {
        ContinueButton.onClick.AddListener(ContinueGame);
        OptionsButton.onClick.AddListener(OpenOptions);
        ExitToMenuButton.onClick.AddListener(ExitGame);
        pauseMenuUI.SetActive(false);
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
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsPaused = false;
    }

    public void OpenOptions()
    {
        NavigationManager.PreviousScene = SceneManager.GetActiveScene().name;
        NavigationManager.CameFromPause = true;
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}