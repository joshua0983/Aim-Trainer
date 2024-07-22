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

    private float sensitivity = 0.5f; // Default sensitivity value
    private const float sensitivityStep = 0.01f;
    private const float minSensitivity = 0.01f;
    private const float maxSensitivity = 1.0f;

    void Start()
    {
        // Initialize the sensitivity value
        UpdateSensitivityText();

        // Add listeners to the buttons
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
        SceneManager.LoadScene("MainMenu");
    }
}