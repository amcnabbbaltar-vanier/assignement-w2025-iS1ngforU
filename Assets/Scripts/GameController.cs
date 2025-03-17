using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverScreen gameOverScreen; // Reference to the GameOver UI

    public void GameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveFinalTime(); // Save the time before game over
            int finalScore = GameManager.Instance.GetScore(); // Get final score
            float finalTime = GameManager.Instance.GetFinalTime(); // Get final time

            // ✅ Pass score and time to GameOverScreen using Setup()
            gameOverScreen.Setup(finalScore, finalTime);
        }
    }
}
