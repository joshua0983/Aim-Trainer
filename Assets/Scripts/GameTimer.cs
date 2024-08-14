using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float totalTime = 60f;
    private float currentTime;
    private bool isPaused = false;

    private void Start()
    {
        currentTime = totalTime;
        UpdateTimeUI();
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (currentTime > 0)
        {
            if (!isPaused)
            {
                currentTime -= Time.deltaTime;
                UpdateTimeUI();
            }

            yield return null;
        }

        TimeUp();
    }

    private void UpdateTimeUI()
    {
        timeText.text = Mathf.Ceil(currentTime).ToString();
    }

    private void TimeUp()
    {
        int finalScore = ScoreManager.Instance.GetScore();
        PlayerPrefs.SetInt("FinalScore", finalScore);
        SceneManager.LoadScene("ResultScene");
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }
}