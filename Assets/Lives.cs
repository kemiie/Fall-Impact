using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class PlayerLives : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Image[] heartImages;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnProtectionTime = 0.5f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private MonoBehaviour movementScript;

    private int currentLives;
    private Rigidbody rb;
    private bool isRespawning;

    private void Awake()
    {
        currentLives = maxLives;
        rb = GetComponent<Rigidbody>();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateHeartUI();
    }

    public void LoseHeart()
    {
        if (isRespawning || currentLives <= 0)
        {
            return;
        }

        currentLives--;
        UpdateHeartUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer()
    {
        isRespawning = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }
        else
        {
            Debug.LogWarning("Respawn Point has not been assigned.");
        }

        yield return new WaitForSeconds(respawnProtectionTime);

        isRespawning = false;
    }

    private void GameOver()
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("Game Over");
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < currentLives;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}