using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameManager gameManager;
    private Timer timer;
    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.Instance;
        timer = FindObjectOfType<Timer>(); 
    }

    public void StartGame()
    {
        if (gameManager != null)
        {
            gameManager.playerHealth = 3; // Reset health
            gameManager.ResetScore(); // Reset score
        }

        if (timer != null)
        {
            timer.ResetTimer(); // Reset the timer
        }

        SceneManager.LoadScene(1); // Load first game scene
    }

}
