using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Button backButton;
    public Button retryButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        resultText.text = finalScore.ToString();

        backButton.onClick.AddListener(BackToMainMenu);
        retryButton.onClick.AddListener(RetryGame);
    }

    public void RetryGame()
    {
        string gameMode = PlayerPrefs.GetString("SelectedGameMode");
        string difficulty = PlayerPrefs.GetString("SelectedDifficulty");

        SceneManager.LoadScene(gameMode);

        DifficultyScript.difficulty = difficulty;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}