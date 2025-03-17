using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Slider healthBar;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            currentHealth = gameManager.playerHealth; // Load health from GameManager
        }
        else
        {
            currentHealth = maxHealth;
        }

        SetMaxHealth(maxHealth);
        SetHealth(currentHealth);
    }

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        SetHealth(currentHealth);
        
        if (gameManager != null)
        {
            gameManager.playerHealth = currentHealth; // Save health to GameManager
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene("EndScene");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
        {
            TakeDamage(1);
            Respawn();
        }
    }

    void Respawn()
    {
        // Reset position to last checkpoint or spawn point
        transform.position = gameManager.spawnPoint; 
    }
}
