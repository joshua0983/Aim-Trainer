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

    public GameObject optionsMenuUI;
    public GameObject pauseMenuUI;

    private float sensitivity;
    private const float sensitivityStep = 0.01f;
    private const float minSensitivity = 0.01f;
    private const float maxSensitivity = 1.0f;

    public static bool CameFromGame = false;

    private PlayerLook playerLook;

    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
        UpdateSensitivityText();

        increaseButton.onClick.AddListener(IncreaseSensitivity);
        decreaseButton.onClick.AddListener(DecreaseSensitivity);
        backButton.onClick.AddListener(BackButton);

        playerLook = FindObjectOfType<PlayerLook>();
        if (playerLook != null)
        {
            playerLook.SetSensitivity(sensitivity);
        }
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
            UpdatePlayerLookSensitivity();
        }
    }

    void DecreaseSensitivity()
    {
        if (sensitivity > minSensitivity)
        {
            sensitivity -= sensitivityStep;
            UpdateSensitivityText();
            UpdatePlayerLookSensitivity();
        }
    }

    void UpdatePlayerLookSensitivity()
    {
        if (playerLook != null)
        {
            playerLook.SetSensitivity(sensitivity);
        }
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    void BackButton()
    {
        if (CameFromGame)
        {
            optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameFromGame = false;
        }
        else
        {
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