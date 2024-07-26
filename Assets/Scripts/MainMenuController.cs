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