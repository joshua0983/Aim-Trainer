using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeSelectionScript : MonoBehaviour
{
    public Button Flicking;
    public Button EnemyShooting;
    public Button BackButton;
    public Toggle toggle;
    public static string GameMode;
    public static bool StrafeEnabled { get; private set; }

    void Start()
    {
        StrafeEnabled = PlayerPrefs.GetInt("StrafeEnabled", 0) == 1;
        toggle.isOn = StrafeEnabled;

        Flicking.onClick.AddListener(FlickingButton);
        BackButton.onClick.AddListener(GameModeSelectionBackButton);
        EnemyShooting.onClick.AddListener(EnemyShootingButton);
        toggle.onValueChanged.AddListener(onToggleValueChanged);
    }

    void GameModeSelectionBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void FlickingButton()
    {
        GameMode = "Flicking";
        PlayerPrefs.SetString("SelectedGameMode", GameMode);
        SceneManager.LoadScene("Difficulty");
    }

    public void EnemyShootingButton()
    {
        GameMode = "EnemyShooting";
        PlayerPrefs.SetString("SelectedGameMode", GameMode);
        SceneManager.LoadScene("Difficulty");
    }

    public void onToggleValueChanged(bool value)
    {
        StrafeEnabled = value;
        PlayerPrefs.SetInt("StrafeEnabled", value ? 1 : 0);
    }
}