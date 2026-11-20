
using UnityEngine;
using System.Collections.Generic;

public class PetManager : MonoBehaviour
{
    public static PetManager Instance { get; private set; }

    [Header("Owned Pets")]
    public List<string> petNames = new List<string>();
    public List<int> petRarities = new List<int>();

    [Header("Equipped Pets")]
    public List<int> equippedPetIndices = new List<int>();
    public int maxEquippedPets = 5;

    [Header("Pet Following")]
    public Transform player;
    public List<GameObject> petPrefabs = new List<GameObject>();

    private List<GameObject> spawnedPets =
        new List<GameObject>();

    [Header("Pet Formation")]
    public float followDistance = 2f;
    public float petSpacing = 1.5f;
    public float followSpeed = 5f;

    [Header("Money")]
    public double unclaimedCoins = 0;

    private float uiTimer;

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

    private void Start()
    {
        EquipBestPets();
    }

    private void Update()
    {
        // ALL owned pets generate money.
        unclaimedCoins += GetCoinsPerMinute()
            * Time.deltaTime / 60.0;

        uiTimer += Time.deltaTime;

        if (uiTimer >= 1f)
        {
            uiTimer = 0f;

            if (InventoryManager.Instance != null)
                InventoryManager.Instance.UpdateIncomeUI();
        }
    }

    private void LateUpdate()
    {
        UpdatePetFollowing();
    }

    private void FixLists()
    {
        while (petRarities.Count < petNames.Count)
            petRarities.Add(3);

        while (petRarities.Count > petNames.Count)
            petRarities.RemoveAt(petRarities.Count - 1);
    }

    public void AddPet(string petName, int rarity)
    {
        FixLists();

        petNames.Add(petName);
        petRarities.Add(rarity);

        Debug.Log("[PETS] Collected " + petName);

        EquipBestPets();

        UpdateAllUI();
    }

    public void EquipBestPets()
    {
        FixLists();

        equippedPetIndices.Clear();

        List<int> sortedIndices = new List<int>();

        for (int i = 0; i < petNames.Count; i++)
            sortedIndices.Add(i);

        sortedIndices.Sort((a, b) =>
        {
            int result =
                petRarities[b].CompareTo(petRarities[a]);

            if (result != 0)
                return result;

            return b.CompareTo(a);
        });

        int amount = Mathf.Min(
            maxEquippedPets,
            sortedIndices.Count
        );

        for (int i = 0; i < amount; i++)
        {
            equippedPetIndices.Add(sortedIndices[i]);
        }

        SpawnEquippedPets();

        Debug.Log("[PETS] Equipped " + amount + " best pets!");
    }

    private void SpawnEquippedPets()
    {
        // Remove previous pet objects.
        foreach (GameObject pet in spawnedPets)
        {
            if (pet != null)
                Destroy(pet);
        }

        spawnedPets.Clear();

        if (player == null)
            return;

        foreach (int index in equippedPetIndices)
        {
            if (index < 0 || index >= petNames.Count)
                continue;

            int rarity = petRarities[index];

            // Prefab index uses rarity:
            // 3 = Common, 4 = Rare, 5 = Epic,
            // 6 = Legendary, 7 = Mythical.
            int prefabIndex = rarity - 3;

            if (prefabIndex < 0 ||
                prefabIndex >= petPrefabs.Count)
                continue;

            GameObject prefab = petPrefabs[prefabIndex];

            if (prefab == null)
                continue;

            GameObject newPet = Instantiate(
                prefab,
                player.position,
                Quaternion.identity
            );

            spawnedPets.Add(newPet);
        }
    }

    private void UpdatePetFollowing()
    {
        if (player == null)
            return;

        for (int i = 0; i < spawnedPets.Count; i++)
        {
            if (spawnedPets[i] == null)
                continue;

            float angle =
                (360f / Mathf.Max(1, spawnedPets.Count)) * i;

            float radians = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(radians) * followDistance,
                0f,
                Mathf.Sin(radians) * followDistance
            );

            Vector3 target = player.position + offset;

            spawnedPets[i].transform.position =
                Vector3.Lerp(
                    spawnedPets[i].transform.position,
                    target,
                    followSpeed * Time.deltaTime
                );
        }
    }

    public int GetCoinsPerMinute()
    {
        FixLists();

        int total = 0;

        for (int i = 0; i < petNames.Count; i++)
        {
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

    public int GetClaimableCoins()
    {
        return Mathf.FloorToInt((float)unclaimedCoins);
    }

    public void ClaimCoins()
    {
        int reward = GetClaimableCoins();

        if (reward <= 0)
            return;

        if (MoneySystem.Instance == null)
            return;

        MoneySystem.Instance.money += reward;

        CharacterRewardBoard board =
            FindObjectOfType<CharacterRewardBoard>();

        if (board != null)
            board.UpdateCoinText();

        unclaimedCoins -= reward;

        UpdateAllUI();
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

    public int GetPetIncome(int rarity)
    {
        switch (rarity)
        {
            case 3: return 3;
            case 4: return 5;
            case 5: return 10;
            case 6: return 20;
            case 7: return 30;
            default: return 0;
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
            result += GetRarityName(petRarities[i]) + "\n";
            result += "+" + GetPetIncome(petRarities[i]) +
                " COINS PER MIN\n\n";
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