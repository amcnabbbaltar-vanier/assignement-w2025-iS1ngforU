using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerHealth = 3;
    public Vector3 spawnPoint;
    public int score = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    public float finalTime = 0f;

    private Timer timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        FindScoreText();
    }

    private void Start()
    {
        UpdateScoreUI();
        timer = FindObjectOfType<Timer>();

        if (timer != null)
        {
            timer.StartTimer(); // ✅ Ensure timer starts
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            spawnPoint = player.transform.position;
        }
    }

    public void SaveFinalTime()
    {
        if (timer != null)
        {
            finalTime = timer.GetElapsedTime();
            Debug.Log("Final Time Saved: " + finalTime);
        }
    }

    public float GetFinalTime()
    {
        return finalTime;
    }

    private void FindScoreText()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform scoreTextTransform = canvas.transform.Find("ScoreText");
            if (scoreTextTransform != null)
            {
                scoreText = scoreTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }

        if (scoreText == null)
        {
            Debug.LogWarning("ScoreText not found in Canvas!");
        }
        else
        {
            UpdateScoreUI();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        ResetScore();

        if (timer != null)
        {
            timer.ResetTimer(); // ✅ Reset timer on loading main menu
        }

        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetScore();

        if (timer != null)
        {
            timer.ResetTimer(); // ✅ Reset timer on restart
            timer.StartTimer();
        }

        SceneManager.LoadScene(1);
        playerHealth = 3;
    }

    public void UpdateSpawnPoint(Vector3 newSpawn)
    {
        spawnPoint = newSpawn;
    }

    public void PlayerDied()
    {
        playerHealth--;

        if (playerHealth <= 0)
        {
            if (timer != null)
            {
                timer.StopTimer();
                SaveFinalTime();
            }

            SceneManager.LoadScene("EndScene");
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint;
            }
        }
    }
}
