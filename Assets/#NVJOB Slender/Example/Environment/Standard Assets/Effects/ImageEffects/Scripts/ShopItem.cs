using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("Item Information")]
    public string itemName;
    public int price;

    [Header("Inventory")]
    public Sprite itemIcon;
    public InventorySlot inventorySlot;

    [Header("3D Item")]
    public GameObject itemPrefab;
    public Transform spawnPoint;

    [Header("Message")]
    public TextMeshProUGUI messageText;

    [Header("Speed Item")]
    public bool isSpeedItem = false;
    public float speedMultiplier = 2f;
    public float speedDuration = 30f;

    [Header("Balloon / Flight Item")]
    public bool isFlightItem = false;
    public float flightDuration = 15f;

    public void BuyItem()
    {
        // Check money
        if (MoneySystem.Instance.money >= price)
        {
            // Take money
            MoneySystem.Instance.SpendMoney(price);

            // Add item to inventory
            if (inventorySlot != null && itemIcon != null)
            {
                inventorySlot.AddItem(itemIcon);
            }

            // Spawn the 3D item
            if (itemPrefab != null && spawnPoint != null)
            {
                Instantiate(
                    itemPrefab,
                    spawnPoint.position,
                    spawnPoint.rotation
                );
            }

            // Find player
            MOVING playerMovement =
                FindFirstObjectByType<MOVING>();

            // Speed item
            if (isSpeedItem && playerMovement != null)
            {
                playerMovement.ActivateSpeedBoost(
                    speedMultiplier,
                    speedDuration
                );
            }

            // Balloon item
            if (isFlightItem && playerMovement != null)
            {
                playerMovement.ActivateFlight(
                    flightDuration
                );
            }

            // Success message
            if (messageText != null)
            {
                messageText.text =
                    "You bought " + itemName + "!";
            }

            Debug.Log("Bought " + itemName);
        }
        else
        {
            // Not enough money
            if (messageText != null)
            {
                messageText.text =
                    "You don't have enough money";
            }

            Debug.Log("Not enough coins!");
        }
    }
}