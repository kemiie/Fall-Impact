
using UnityEngine;
using System.Collections.Generic;

public class PetManager : MonoBehaviour
{
    public static PetManager Instance { get; private set; }

    [Header("Owned Pets")]
    public List<string> petNames = new List<string>();
    public List<int> petRarities = new List<int>();

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

    private void Update()
    {
        // ALL owned pets generate money automatically.
        unclaimedCoins += GetCoinsPerMinute() * Time.deltaTime / 60.0;

        // Refresh UI once per second.
        uiTimer += Time.deltaTime;

        if (uiTimer >= 1f)
        {
            uiTimer = 0f;

            if (InventoryManager.Instance != null)
                InventoryManager.Instance.UpdateIncomeUI();
        }
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

        UpdateAllUI();
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
        {
            Debug.LogError("[PETS] MoneySystem not found!");
            return;
        }

        MoneySystem.Instance.money += reward;

        CharacterRewardBoard board =
    FindObjectOfType<CharacterRewardBoard>();

        if (board != null)
            board.UpdateCoinText();

        unclaimedCoins -= reward;

        Debug.Log("[PETS] Claimed " + reward + " coins!");

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
                      " COINS PER MINUTE\n\n";
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