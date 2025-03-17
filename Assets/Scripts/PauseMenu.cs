using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public GameObject PauseMenuUI;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                //checks if game has already been paused and will resume it 
                Resume();
            }
            else
            {
                // if not pause when pressed esc key it will pause it
                Pause();
            }
        }
    }

    public void Resume()
    {
        //disables or enables gameObject
        PauseMenuUI.SetActive(false);
        //unfreeze time
        Time.timeScale = 1f;
        //change value for game is pause so when we go to our condition statement it will know if its paused or not
        GameIsPause = false;
    }

    void Pause()
    {
        //disables or enables gameObject
        PauseMenuUI.SetActive(true);
        //freeze time
        Time.timeScale = 0f;
        //change value for game is pause so when we go to our condition statement it will know if its paused or not
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
