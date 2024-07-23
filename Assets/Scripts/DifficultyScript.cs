using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour
{
    public Button backButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    void Start()
    {
        easyButton.onClick.AddListener(EasyButton);
        mediumButton.onClick.AddListener(MediumButton);
        hardButton.onClick.AddListener(HardButton);
        backButton.onClick.AddListener(DifficultyBackButton);
    }

    public void EasyButton()
    {
        RedDot.difficulty = "Easy";
        SceneManager.LoadScene(GameModeSelectionScript.GameMode);
    }
    public void MediumButton()
    {
        RedDot.difficulty = "Medium";
        SceneManager.LoadScene(GameModeSelectionScript.GameMode);
    }
    public void HardButton()
    {
        RedDot.difficulty = "Hard";
        SceneManager.LoadScene(GameModeSelectionScript.GameMode);
    }
    public void DifficultyBackButton()
    {
        SceneManager.LoadScene("GameModeSelection");
    }
}
