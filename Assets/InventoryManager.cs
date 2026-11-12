
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

    [Header("Inventory UI")]
    public TMP_Text inventoryText;
    public TMP_Text petIncomeText;

    [Header("Pet Buttons")]
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
        {
            Debug.LogError("[INVENTORY] PetManager not found!");
            return;
        }

        Debug.Log(
            "[INVENTORY] Owned pets: " +
            manager.petNames.Count
        );

        if (inventoryText != null)
        {
            inventoryText.text =
                manager.GetPetCollectionText();
        }
        else
        {
            Debug.LogError(
                "[INVENTORY] Inventory Text is not assigned!"
            );
        }

        if (petIncomeText != null)
        {
            petIncomeText.text =
                "EQUIPPED: " +
                manager.GetEquippedCount() +
                " / " + manager.maxEquippedPets +
                "\nINCOME: " +
                manager.GetCoinsPerMinute() +
                " COINS PER MINUTE";
        }

        CreatePetButtons();
    }

    private void CreatePetButtons()
    {
        PetManager manager = PetManager.Instance;

        if (manager == null)
            return;

        if (petButtonContainer == null)
        {
            Debug.LogError(
                "[INVENTORY] Pet Button Container is missing!"
            );
            return;
        }

        if (petButtonPrefab == null)
        {
            Debug.LogError(
                "[INVENTORY] Pet Button Prefab is missing!"
            );
            return;
        }

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

            RefreshButtonText(
                buttonText,
                manager,
                petIndex
            );

            newButton.interactable = true;

            newButton.onClick.RemoveAllListeners();

            newButton.onClick.AddListener(() =>
            {
                Debug.Log(
                    "[BUTTON TEST] PET BUTTON CLICKED! " +
                    manager.petNames[petIndex]
                );

                PetManager current = PetManager.Instance;

                if (current == null)
                {
                    Debug.LogError(
                        "[BUTTON ERROR] PetManager is missing!"
                    );
                    return;
                }

                if (petIndex < 0 ||
                    petIndex >= current.petNames.Count)
                {
                    Debug.LogError(
                        "[BUTTON ERROR] Invalid pet index!"
                    );
                    return;
                }

                if (current.equippedPets[petIndex])
                {
                    current.UnequipPet(petIndex);
                }
                else
                {
                    current.EquipPet(petIndex);
                }

                UpdateInventoryUI();
            });
        }

        Debug.Log(
            "[INVENTORY] Created " +
            generatedButtons.Count +
            " pet buttons."
        );
    }

    private void RefreshButtonText(
        TMP_Text buttonText,
        PetManager manager,
        int index)
    {
        if (buttonText == null)
        {
            Debug.LogError(
                "[INVENTORY] Button has no TMP text child!"
            );
            return;
        }

        string petName = manager.petNames[index];

        string rarity =
            manager.GetRarityName(manager.petRarities[index]);

        bool equipped = manager.equippedPets[index];

        buttonText.text =
            petName + " (" + rarity + ")\n" +
            (equipped ? "UNEQUIP" : "EQUIP");
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

        if (Instance == this)
            Instance = null;
    }
}