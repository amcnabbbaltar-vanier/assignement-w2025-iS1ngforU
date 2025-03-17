using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public string playerTag = "Player";
    public string pointsTag = "Points";
    public string enemyTag = "Enemy";
    public string jumpBoostTag = "JumpBoost";
    public string speedBoostTag = "SpeedBoost";

    public ParticleSystem particleSystem;

    void Start()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (CompareTag(pointsTag))
            {
                HandlePointsCollision();
            }
            else if (CompareTag(enemyTag))
            {
                HandleEnemyCollision(other);
            }
            else if (CompareTag(jumpBoostTag))
            {
                HandleJumpBoostCollision(other);
            }
            else if (CompareTag(speedBoostTag))
            {
                HandleSpeedBoostCollision(other);
            }
        }
    }

    private void HandlePointsCollision()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(50);
            Debug.Log("Points added! Current Score: " + GameManager.Instance.GetScore());
        }

        Destroy(gameObject);
        PlayParticles();
    }

    private void HandleEnemyCollision(Collider enemy)
    {
        Debug.Log("Player collided with an enemy!");

        PlayerHealth playerHealth = enemy.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }

        Destroy(gameObject);
        PlayParticles();
    }

    private void HandleJumpBoostCollision(Collider player)
    {
        Debug.Log("Jump Boost Picked Up!");

        Destroy(gameObject);
        PlayParticles();
    }

    private void HandleSpeedBoostCollision(Collider player)
    {
        Debug.Log("Speed Boost Picked Up!");

        Destroy(gameObject);
        PlayParticles();
    }

    private void PlayParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

}
