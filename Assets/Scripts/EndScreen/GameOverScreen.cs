using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            int finalScore = GameManager.Instance.GetScore();
            float finalTime = GameManager.Instance.GetFinalTime();

            // Display final score and time
            finalScoreText.text = "Final Score: " + finalScore;
            finalTimeText.text = "Final Time: " + FormatTime(finalTime);
        }
    }

    // ✅ Setup method to allow dynamic updates from GameController
    public void Setup(int finalScore, float finalTime)
    {
        finalScoreText.text = "Final Score: " + finalScore;
        finalTimeText.text = "Final Time: " + FormatTime(finalTime);

        // Show the GameOver UI
        gameObject.SetActive(true);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth = 3;
            GameManager.Instance.ResetScore(); // ✅ Reset score
        }

        // Reset the timer
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.ResetTimer();
            timer.StartTimer(); // ✅ Restart timer
        }

        SceneManager.LoadScene(1); // Load game scene
    }

    public void MainMenuButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth = 3;
            GameManager.Instance.ResetScore(); // ✅ Reset score
        }

        // Reset the timer
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.ResetTimer();
        }

        SceneManager.LoadScene(0); // Load Main Menu scene
    }
}
