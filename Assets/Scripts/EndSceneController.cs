using System.Collections;
using TMPro;
using UnityEngine;

public class EndSceneController : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText; // UI Text for the final time

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            float finalTime = GameManager.Instance.GetFinalTime();
            finalTimeText.text = "Time Played: " + FormatTime(finalTime);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
