using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    public TextMeshProUGUI sensitivityText;
    public Button increaseButton;
    public Button decreaseButton;
    public Button backButton;

    public GameObject optionsMenuUI; // Reference to options menu UI
    public GameObject pauseMenuUI; // Reference to pause menu UI

    private float sensitivity = 0.5f;
    private const float sensitivityStep = 0.01f;
    private const float minSensitivity = 0.01f;
    private const float maxSensitivity = 1.0f;

    public static bool CameFromGame = false; // Flag to indicate if options were opened from the game

    void Start()
    {
        UpdateSensitivityText();
        increaseButton.onClick.AddListener(IncreaseSensitivity);
        decreaseButton.onClick.AddListener(DecreaseSensitivity);
        backButton.onClick.AddListener(BackButton);
    }

    void UpdateSensitivityText()
    {
        sensitivityText.text = sensitivity.ToString("F2");
    }

    void IncreaseSensitivity()
    {
        if (sensitivity < maxSensitivity)
        {
            sensitivity += sensitivityStep;
            UpdateSensitivityText();
        }
    }

    void DecreaseSensitivity()
    {
        if (sensitivity > minSensitivity)
        {
            sensitivity -= sensitivityStep;
            UpdateSensitivityText();
        }
    }

    void BackButton()
    {
        if (CameFromGame)
        {
            // If we came from the game, switch back to the pause menu
            optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameFromGame = false;
        }
        else
        {
            // Otherwise, go back to the main menu
            if (!string.IsNullOrEmpty(NavigationManager.PreviousScene))
            {
                SceneManager.LoadScene(NavigationManager.PreviousScene);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}