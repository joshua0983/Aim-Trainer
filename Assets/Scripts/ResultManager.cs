using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    private void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        resultText.text = finalScore.ToString();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("GameModeSelection");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}