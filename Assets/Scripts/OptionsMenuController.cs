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

    private float sensitivity = 0.5f;
    private const float sensitivityStep = 0.01f;
    private const float minSensitivity = 0.01f;
    private const float maxSensitivity = 1.0f;

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
        if (!string.IsNullOrEmpty(NavigationManager.PreviousScene))
        {
            InputManager.ShouldStartPaused = true; // Set the flag to true
            SceneManager.LoadScene(NavigationManager.PreviousScene);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}