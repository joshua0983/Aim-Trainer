using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Button PlayButton;
    public Button OptionsButton;
    public Button ExitButton;

    void Start()
    {
        // Check if buttons are assigned in the Inspector
        if (PlayButton == null || OptionsButton == null || ExitButton == null)
        {
            Debug.LogError("One or more buttons are not assigned in the Inspector.");
            return;
        }

        PlayButton.onClick.AddListener(PlayGame);
        OptionsButton.onClick.AddListener(OpenOptions);
        ExitButton.onClick.AddListener(ExitGame);
    }

    void PlayGame()
    {
        SceneManager.LoadScene("GameModeSelection");
    }

    void OpenOptions()
    {
        NavigationManager.PreviousScene = "MainMenu";
        SceneManager.LoadScene("OptionsMenu");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}