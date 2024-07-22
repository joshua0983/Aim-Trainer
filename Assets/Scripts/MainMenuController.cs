using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameModeSelection");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}