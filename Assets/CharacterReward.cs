using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class CharacterRewardBoard : MonoBehaviour
{
    [Header("Reward Board")]
    public TMP_Text rewardBoardText;
    public Button claimAllButton;

    [Header("Coin Text")]
    public TMP_Text coinText;

    [Header("Rarity Rewards")]
    public int commonCoins = 5;
    public int rareCoins = 15;
    public int epicCoins = 40;
    public int legendaryCoins = 100;
    public int mythicalCoins = 250;

    private List<string> pendingRewards = new List<string>();
    private int pendingCoins;

    private void Start()
    {
        if (claimAllButton != null)
            claimAllButton.onClick.AddListener(ClaimAll);

        UpdateBoard();
        UpdateCoinText();
    }

    // ADD CHARACTER AFTER A PULL
    public void AddPulledCharacter(string characterName, int rarity)
    {
        int reward = GetCoinReward(rarity);

        if (reward <= 0)
            return;

        pendingRewards.Add(
            characterName + " - " +
            GetRarityName(rarity) +
            " - +" + reward + " Coins"
        );

        pendingCoins += reward;

        UpdateBoard();
    }

    // GET COINS BASED ON RARITY
    private int GetCoinReward(int rarity)
    {
        switch (rarity)
        {
            case 3: return commonCoins;
            case 4: return rareCoins;
            case 5: return epicCoins;
            case 6: return legendaryCoins;
            case 7: return mythicalCoins;
            default: return 0;
        }
    }

    // CLAIM ALL REWARDS
    public void ClaimAll()
    {
        if (pendingCoins <= 0)
            return;

        if (MoneySystem.Instance == null)
        {
            Debug.LogError("[REWARDS] MoneySystem not found!");
            return;
        }

        MoneySystem.Instance.money += pendingCoins;

        Debug.Log("[REWARDS] Claimed " + pendingCoins + " coins!");

        pendingCoins = 0;
        pendingRewards.Clear();

        UpdateBoard();
        UpdateCoinText();
    }

    // UPDATE REWARD BOARD
    private void UpdateBoard()
    {
        if (rewardBoardText == null)
            return;

        StringBuilder display = new StringBuilder();

        display.AppendLine("CHARACTER REWARDS");
        display.AppendLine();

        if (pendingRewards.Count == 0)
        {
            display.AppendLine("No rewards available!");
        }
        else
        {
            foreach (string reward in pendingRewards)
            {
                display.AppendLine(reward);
            }
        }

        display.AppendLine();
        display.AppendLine("--------------------");
        display.AppendLine("TOTAL: " + pendingCoins + " COINS");

        rewardBoardText.text = display.ToString();

        if (claimAllButton != null)
            claimAllButton.interactable = pendingCoins > 0;
    }

    // UPDATE PLAYER COIN TEXT
    public void UpdateCoinText()
    {
        if (coinText != null && MoneySystem.Instance != null)
        {
            coinText.text = MoneySystem.Instance.money.ToString();
        }
    }

    private string GetRarityName(int rarity)
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

    private void OnDestroy()
    {
        if (claimAllButton != null)
            claimAllButton.onClick.RemoveListener(ClaimAll);
    }
}