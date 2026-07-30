using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class WaterGame : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int startingHearts = 3;
    [SerializeField] private Image[] heartImages;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float protectionTime = 0.5f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    private int currentHearts;
    private bool isRespawning;
    private bool isGameOver;

    private CharacterController controller;
    private Behaviour movementScript;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        movementScript =
            GetComponent("SimplePlayerMovement") as Behaviour;

        currentHearts = startingHearts;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateHearts();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Water"))
        {
            LoseHeart();
        }
    }

    private void LoseHeart()
    {
        if (isRespawning || isGameOver)
        {
            return;
        }

        currentHearts--;
        UpdateHearts();

        Debug.Log("Hearts remaining: " + currentHearts);

        if (currentHearts <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;

        controller.enabled = false;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }
        else
        {
            Debug.LogError("Respawn Point is missing.");
        }

        controller.enabled = true;

        yield return new WaitForSeconds(protectionTime);

        isRespawning = false;
    }

    private void GameOver()
    {
        isGameOver = true;

        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game Over");
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].gameObject.SetActive(
                    i < currentHearts
                );
            }
        }
    }
}