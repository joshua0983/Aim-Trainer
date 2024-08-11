using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour
{
    public Button backButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public static string difficulty; 
    
    void Start()
    {
        easyButton.onClick.AddListener(EasyButton);
        mediumButton.onClick.AddListener(MediumButton);
        hardButton.onClick.AddListener(HardButton);
        backButton.onClick.AddListener(DifficultyBackButton);
    }

    public void EasyButton()
    {
        SetDifficulty("Easy");
        SceneManager.LoadScene(GameModeSelectionScript.GameMode);
    }

    public void MediumButton()
    {
        SetDifficulty("Medium");
        SceneManager.LoadScene(GameModeSelectionScript.GameMode);
    }

    public void HardButton()
    {
        SetDifficulty("Hard");
        SceneManager.LoadScene(GameModeSelectionScript.GameMode);
    }

    private void SetDifficulty(string selectedDifficulty)
    {
        difficulty = selectedDifficulty;
        PlayerPrefs.SetString("SelectedDifficulty", difficulty); // Store the selected difficulty
    }

    public void DifficultyBackButton()
    {
        SceneManager.LoadScene("GameModeSelection");
    }
}