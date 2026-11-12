
using UnityEngine;
using System.Collections.Generic;

public class PetManager : MonoBehaviour
{
    public static PetManager Instance { get; private set; }

    [Header("Owned Pets")]
    public List<string> petNames = new List<string>();
    public List<int> petRarities = new List<int>();
    public List<bool> equippedPets = new List<bool>();

    [Header("Settings")]
    public int maxEquippedPets = 3;

    private float coinTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        FixLists();
    }

    private void Update()
    {
        if (GetCoinsPerMinute() <= 0)
            return;

        coinTimer += Time.deltaTime;

        if (coinTimer >= 60f)
        {
            int minutes = Mathf.FloorToInt(coinTimer / 60f);
            coinTimer -= minutes * 60f;

            if (MoneySystem.Instance != null)
            {
                int reward = GetCoinsPerMinute() * minutes;
                MoneySystem.Instance.money += reward;

                Debug.Log("[PETS] Earned " + reward + " coins!");
            }
        }
    }

    private void FixLists()
    {
        while (petRarities.Count < petNames.Count)
            petRarities.Add(3);

        while (equippedPets.Count < petNames.Count)
            equippedPets.Add(false);

        while (petRarities.Count > petNames.Count)
            petRarities.RemoveAt(petRarities.Count - 1);

        while (equippedPets.Count > petNames.Count)
            equippedPets.RemoveAt(equippedPets.Count - 1);
    }

    public void AddPet(string petName, int rarity)
    {
        FixLists();

        petNames.Add(petName);
        petRarities.Add(rarity);
        equippedPets.Add(false);

        Debug.Log("[PETS] Collected " + petName);

        UpdateAllUI();
    }

    public bool EquipPet(int index)
    {
        FixLists();

        if (index < 0 || index >= petNames.Count)
            return false;

        if (equippedPets[index])
            return false;

        if (GetEquippedCount() >= maxEquippedPets)
        {
            Debug.Log("[PETS] Maximum equipped pets reached!");
            return false;
        }

        equippedPets[index] = true;

        Debug.Log("[PETS] Equipped " + petNames[index]);

        UpdateAllUI();
        return true;
    }

    public void UnequipPet(int index)
    {
        FixLists();

        if (index < 0 || index >= petNames.Count)
            return;

        if (!equippedPets[index])
            return;

        equippedPets[index] = false;

        Debug.Log("[PETS] Unequipped " + petNames[index]);

        UpdateAllUI();
    }

    public int GetEquippedCount()
    {
        FixLists();

        int count = 0;

        foreach (bool equipped in equippedPets)
        {
            if (equipped)
                count++;
        }

        return count;
    }

    public int GetCoinsPerMinute()
    {
        FixLists();

        int total = 0;

        for (int i = 0; i < petNames.Count; i++)
        {
            if (!equippedPets[i])
                continue;

            switch (petRarities[i])
            {
                case 3: total += 3; break;
                case 4: total += 5; break;
                case 5: total += 10; break;
                case 6: total += 20; break;
                case 7: total += 30; break;
            }
        }

        return total;
    }

    public string GetRarityName(int rarity)
    {
        switch (rarity)
        {
            case 3: return "COMMON";
            case 4: return "RARE";
            case 5: return "EPIC";
            case 6: return "LEGENDARY";
            case 7: return "MYTHICAL";
            default: return "UNKNOWN";
        }
    }

    public string GetPetCollectionText()
    {
        FixLists();

        if (petNames.Count == 0)
            return "YOUR COMPANIONS\n\nNo pets collected yet!";

        string result = "YOUR COMPANIONS\n\n";

        for (int i = 0; i < petNames.Count; i++)
        {
            result += petNames[i] + "\n";
            result += GetRarityName(petRarities[i]) + "\n\n";
        }

        return result;
    }

    public void UpdateAllUI()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.UpdateInventoryUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}