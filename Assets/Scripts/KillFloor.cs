using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class KillFloor : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
            {
                SceneManager.LoadScene("SampleScene"); // Replace "DeathScene" with your actual scene name
            }

        
    }
}
