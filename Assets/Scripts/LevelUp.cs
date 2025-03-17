using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour
{
    [SerializeField] private string nextLevel = "Level 2"; // Set this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers it
        {
            Debug.Log("Level Complete! Loading next level...");
            SceneManager.LoadScene(nextLevel); // Load the next level
        }
    }
}
