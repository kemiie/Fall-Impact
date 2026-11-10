using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;

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
        DontDestroyOnLoad(gameObject);
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
        if (PetManager.Instance == null)
            return;

        if (inventoryText != null)
        {
            inventoryText.text =
                PetManager.Instance.GetPetCollectionText();
        }

        if (petIncomeText != null)
        {
            petIncomeText.text =
                "EQUIPPED: " +
                PetManager.Instance.GetEquippedCount() +
                " / 3\n" +
                "INCOME: " +
                PetManager.Instance.GetCoinsPerMinute() +
                " COINS PER MINUTE";
        }

        CreatePetButtons();
    }

    private void CreatePetButtons()
    {
        if (petButtonContainer == null ||
            petButtonPrefab == null)
        {
            Debug.LogWarning(
                "[INVENTORY] Pet button references missing!"
            );
            return;
        }

        foreach (GameObject button in generatedButtons)
        {
            if (button != null)
                Destroy(button);
        }

        generatedButtons.Clear();

        for (int i = 0;
             i < PetManager.Instance.petNames.Count;
             i++)
        {
            int petIndex = i;

            Button newButton = Instantiate(
                petButtonPrefab,
                petButtonContainer
            );

            generatedButtons.Add(newButton.gameObject);

            TMP_Text buttonText =
                newButton.GetComponentInChildren<TMP_Text>();

            string petName =
                PetManager.Instance.petNames[i];

            bool equipped =
                PetManager.Instance.equippedPets[i];

            if (buttonText != null)
            {
                buttonText.text = petName +
                    (equipped ? " - UNEQUIP" : " - EQUIP");
            }

            newButton.onClick.AddListener(() =>
            {
                PetManager manager = PetManager.Instance;

                if (manager == null)
                    return;

                if (manager.equippedPets[petIndex])
                {
                    manager.UnequipPet(petIndex);
                }
                else
                {
                    manager.EquipPet(petIndex);
                }
            });
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
    }
}