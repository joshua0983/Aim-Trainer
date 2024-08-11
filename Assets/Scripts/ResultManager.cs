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

        Debug.Log("Adding listeners to buttons");

        backButton.onClick.AddListener(BackToMainMenu);
        retryButton.onClick.AddListener(RetryGame);
    }

    public void RetryGame()
    {
        Debug.Log("Retry button clicked");

        // Get the last selected game mode and difficulty
        string gameMode = PlayerPrefs.GetString("SelectedGameMode");
        string difficulty = PlayerPrefs.GetString("SelectedDifficulty");

        // Log the selected game mode and difficulty for debugging
        Debug.Log("Retrying game mode: " + gameMode + " with difficulty: " + difficulty);

        // Load the selected game mode scene
        SceneManager.LoadScene(gameMode);

        // Optionally, set the difficulty again (if needed by your game logic)
        DifficultyScript.difficulty = difficulty;
    }

    public void BackToMainMenu()
    {
        Debug.Log("Back button clicked");
        SceneManager.LoadScene("MainMenu");
    }
}