
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PetPrefabEntry
{
    public string petName;
    public GameObject petPrefab;
}

public class PetManager : MonoBehaviour
{
    public static PetManager Instance { get; private set; }

    [Header("Pet Collection")]
    public List<string> petNames = new List<string>();
    public List<int> petRarities = new List<int>();
    public List<bool> equippedPets = new List<bool>();

    [Header("Pet Settings")]
    public int maxEquippedPets = 3;

    [Header("Player")]
    public Transform player;

    [Header("Pet Models")]
    public List<PetPrefabEntry> petPrefabs =
        new List<PetPrefabEntry>();

    [Header("Pet Positions")]
    public Vector3[] petPositions =
    {
        new Vector3(-1f, 0.5f, -1f),
        new Vector3(1f, 0.5f, -1f),
        new Vector3(0f, 0.5f, -2f)
    };

    private List<GameObject> spawnedPets =
        new List<GameObject>();

    private float coinTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        FixPetLists();

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        int income = GetCoinsPerMinute();

        if (income <= 0)
            return;

        coinTimer += Time.deltaTime;

        if (coinTimer >= 60f)
        {
            int minutes = Mathf.FloorToInt(coinTimer / 60f);

            coinTimer -= minutes * 60f;

            if (MoneySystem.Instance != null)
            {
                int reward = income * minutes;

                MoneySystem.Instance.money += reward;

                Debug.Log("[PET INCOME] +" +
                    reward + " coins!");
            }
        }
    }

    // REPAIR PET LISTS
    private void FixPetLists()
    {
        if (petNames == null)
            petNames = new List<string>();

        if (petRarities == null)
            petRarities = new List<int>();

        if (equippedPets == null)
            equippedPets = new List<bool>();

        while (petRarities.Count < petNames.Count)
            petRarities.Add(3);

        while (equippedPets.Count < petNames.Count)
            equippedPets.Add(false);

        while (petRarities.Count > petNames.Count)
            petRarities.RemoveAt(petRarities.Count - 1);

        while (equippedPets.Count > petNames.Count)
            equippedPets.RemoveAt(equippedPets.Count - 1);
    }

    // ADD PET
    public void AddPet(string petName, int rarity)
    {
        FixPetLists();

        if (string.IsNullOrWhiteSpace(petName))
        {
            Debug.LogError("[PET ERROR] Invalid pet name!");
            return;
        }

        petNames.Add(petName);
        petRarities.Add(rarity);
        equippedPets.Add(false);

        spawnedPets.Add(null);

        Debug.Log("[PET ADDED] " + petName);

        UpdateAllUI();
    }

    // EQUIP PET
    public bool EquipPet(int index)
    {
        FixPetLists();

        if (index < 0 || index >= petNames.Count)
        {
            Debug.LogError("[PET ERROR] Invalid pet index!");
            return false;
        }

        if (equippedPets[index])
        {
            Debug.LogWarning("[PET ERROR] Already equipped!");
            return false;
        }

        if (GetEquippedCount() >= maxEquippedPets)
        {
            Debug.LogError("[PET ERROR] Maximum 3 pets equipped!");
            return false;
        }

        equippedPets[index] = true;

        SpawnPet(index);

        Debug.Log("[PET EQUIPPED] " + petNames[index]);

        UpdateAllUI();

        return true;
    }

    // UNEQUIP PET
    public void UnequipPet(int index)
    {
        FixPetLists();

        if (index < 0 || index >= petNames.Count)
        {
            Debug.LogError("[PET ERROR] Invalid pet index!");
            return;
        }

        if (!equippedPets[index])
        {
            Debug.LogWarning("[PET ERROR] Pet is not equipped!");
            return;
        }

        equippedPets[index] = false;

        RemovePetVisual(index);

        Debug.Log("[PET UNEQUIPPED] " + petNames[index]);

        UpdateAllUI();
    }

    // SPAWN PET MODEL
    private void SpawnPet(int index)
    {
        if (player == null)
        {
            Debug.LogError("[PET ERROR] Player is not assigned!");
            return;
        }

        PetPrefabEntry entry = petPrefabs.Find(
            p => p.petName == petNames[index]
        );

        if (entry == null || entry.petPrefab == null)
        {
            Debug.LogWarning(
                "[PET WARNING] No prefab assigned for " +
                petNames[index]
            );

            return;
        }

        int positionIndex = GetEquippedCount() - 1;

        Vector3 offset = petPositions.Length > 0
            ? petPositions[positionIndex % petPositions.Length]
            : Vector3.zero;

        GameObject spawned = Instantiate(
            entry.petPrefab,
            player
        );

        spawned.transform.localPosition = offset;
        spawned.transform.localRotation = Quaternion.identity;

        while (spawnedPets.Count <= index)
            spawnedPets.Add(null);

        spawnedPets[index] = spawned;
    }

    // REMOVE PET MODEL
    private void RemovePetVisual(int index)
    {
        if (index < spawnedPets.Count &&
            spawnedPets[index] != null)
        {
            Destroy(spawnedPets[index]);
            spawnedPets[index] = null;
        }
    }

    // COUNT EQUIPPED PETS
    public int GetEquippedCount()
    {
        FixPetLists();

        int count = 0;

        foreach (bool equipped in equippedPets)
        {
            if (equipped)
                count++;
        }

        return count;
    }

    // COINS PER MINUTE
    public int GetCoinsPerMinute()
    {
        FixPetLists();

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

    // INVENTORY DISPLAY
    public string GetPetCollectionText()
    {
        FixPetLists();

        string display = "YOUR COMPANIONS\n\n";

        if (petNames.Count == 0)
            return display + "No pets yet!";

        for (int i = 0; i < petNames.Count; i++)
        {
            display += petNames[i] + "\n";
            display += GetRarityName(petRarities[i]) + "\n";

            display += equippedPets[i]
                ? "EQUIPPED\n\n"
                : "NOT EQUIPPED\n\n";
        }

        return display;
    }

    // RARITY NAMES
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

    // REFRESH INVENTORY
    public void UpdateAllUI()
    {
        FixPetLists();

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.UpdateInventoryUI();
        }

        Debug.Log(
            "[PET SYSTEM] Equipped: " +
            GetEquippedCount() + "/" + maxEquippedPets +
            " | Income: " + GetCoinsPerMinute() +
            " coins/minute"
        );
    }
}