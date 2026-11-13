
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class GameInventorySlot
{
    public string itemName;
    public string itemType;
    public int quantity;

    public GameInventorySlot(string name, string type)
    {
        itemName = name;
        itemType = type;
        quantity = 1;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Panel")]
    public GameObject inventoryPanel;
    public Button openInventoryButton;
    public Button closeInventoryButton;

    [Header("Pet UI")]
    public TMP_Text inventoryText;
    public TMP_Text petIncomeText;
    public TMP_Text unclaimedCoinsText;

    [Header("Claim Button")]
    public Button claimButton;

    [Header("Pet Display")]
    public Transform petButtonContainer;
    public Button petButtonPrefab;

    [Header("Items")]
    public List<GameInventorySlot> items =
        new List<GameInventorySlot>();

    private List<GameObject> generatedButtons =
        new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (openInventoryButton != null)
            openInventoryButton.onClick.AddListener(OpenInventory);

        if (closeInventoryButton != null)
            closeInventoryButton.onClick.AddListener(CloseInventory);

        if (claimButton != null)
            claimButton.onClick.AddListener(ClaimPetCoins);

        UpdateInventoryUI();
    }

    public void OpenInventory()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        UpdateInventoryUI();
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    public void UpdateInventoryUI()
    {
        PetManager manager = PetManager.Instance;

        if (manager == null)
            return;

        if (inventoryText != null)
            inventoryText.text = manager.GetPetCollectionText();

        CreatePetButtons();

        UpdateIncomeUI();
    }

    // Updates the money and income displays.
    public void UpdateIncomeUI()
    {
        PetManager manager = PetManager.Instance;

        if (manager == null)
            return;

        if (petIncomeText != null)
        {
            petIncomeText.text =
                "TOTAL INCOME: " +
                manager.GetCoinsPerMinute() +
                " COINS PER MINUTE";
        }

        if (unclaimedCoinsText != null)
        {
            unclaimedCoinsText.text =
                "UNCLAIMED COINS: " +
                manager.GetClaimableCoins();
        }

        if (claimButton != null)
        {
            claimButton.interactable =
                manager.GetClaimableCoins() > 0;
        }
    }

    // CLAIM BUTTON
    public void ClaimPetCoins()
    {
        if (PetManager.Instance == null)
            return;

        PetManager.Instance.ClaimCoins();

        UpdateIncomeUI();
    }

    private void CreatePetButtons()
    {
        PetManager manager = PetManager.Instance;

        if (manager == null)
            return;

        if (petButtonContainer == null ||
            petButtonPrefab == null)
            return;

        foreach (GameObject button in generatedButtons)
        {
            if (button != null)
                Destroy(button);
        }

        generatedButtons.Clear();

        for (int i = 0; i < manager.petNames.Count; i++)
        {
            int petIndex = i;

            Button newButton = Instantiate(
                petButtonPrefab,
                petButtonContainer
            );

            generatedButtons.Add(newButton.gameObject);

            TMP_Text buttonText =
                newButton.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                string petName = manager.petNames[petIndex];

                int rarity = manager.petRarities[petIndex];

                int income = manager.GetPetIncome(rarity);

                buttonText.text =
                    petName + "\n" +
                    manager.GetRarityName(rarity) + "\n" +
                    "+" + income + " COINS / MIN";
            }

            // Pet buttons are display-only.
            newButton.interactable = false;
        }
    }

    public bool AddItem(string itemName, string itemType)
    {
        GameInventorySlot existing = items.Find(
            x => x.itemName == itemName &&
                 x.itemType == itemType
        );

        if (existing != null)
        {
            existing.quantity++;
        }
        else
        {
            items.Add(
                new GameInventorySlot(itemName, itemType)
            );
        }

        UpdateInventoryUI();

        return true;
    }

    private void OnDestroy()
    {
        if (openInventoryButton != null)
            openInventoryButton.onClick.RemoveListener(OpenInventory);

        if (closeInventoryButton != null)
            closeInventoryButton.onClick.RemoveListener(CloseInventory);

        if (claimButton != null)
            claimButton.onClick.RemoveListener(ClaimPetCoins);

        if (Instance == this)
            Instance = null;
    }
}