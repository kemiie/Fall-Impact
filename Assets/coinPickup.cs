using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Settings")]
    public int coinValue = 10;

    [Header("Animation")]
    public float rotationSpeed = 100f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.25f;

    private Vector3 startPosition;

    private bool collected = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the coin.
        transform.Rotate(
            Vector3.up * rotationSpeed * Time.deltaTime
        );

        // Make the coin float up and down.
        float newY = startPosition.y +
            Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (MoneySystem.Instance == null)
        {
            Debug.LogError("[COIN] MoneySystem not found!");
            return;
        }

        collected = true;

        // Give the player coins.
        MoneySystem.Instance.money += coinValue;

        Debug.Log("[COIN] Collected " + coinValue + " coins!");

        // Update the existing coin display.
        CharacterRewardBoard board =
            FindObjectOfType<CharacterRewardBoard>();

        if (board != null)
            board.UpdateCoinText();

        // Remove the coin.
        Destroy(gameObject);
    }
}