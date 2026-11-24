using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureOfferingQuest : MonoBehaviour
{
    public static TreasureOfferingQuest Instance;

    [Header("Treasure Sigils")]
    public int totalChestsCollected = 0;
    public int chestsReturned = 0;
    public int maxChests = 100;

    [Header("Offering Milestones")]
    public int[] milestones =
    {
        5, 10, 20, 30, 40, 50, 75, 100
    };

    [Header("Coin Rewards")]
    public int[] coinRewards =
    {
        100, 200, 350, 500,
        700, 1000, 1500, 2500
    };

    [Header("EXP Rewards")]
    public int[] expRewards =
    {
        50, 100, 150, 200,
        250, 300, 400, 600
    };

    [Header("Offering UI")]
    public TMP_Text questText;
    public TMP_Text progressText;
    public Button returnButton;

    private int nextMilestoneIndex = 0;

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
        if (returnButton != null)
            returnButton.onClick.AddListener(ReturnChests);

        UpdateQuestUI();
    }

    public void CollectChest()
    {
        if (totalChestsCollected >= maxChests)
            return;

        totalChestsCollected++;

        Debug.Log("[OFFERING] Treasure Sigil collected! " +
            totalChestsCollected + "/" + maxChests);

        UpdateQuestUI();
    }

    public void ReturnChests()
    {
        if (nextMilestoneIndex >= milestones.Length)
            return;

        int required = milestones[nextMilestoneIndex];

        if (totalChestsCollected < required)
            return;

        int amountToReturn = required - chestsReturned;

        if (amountToReturn <= 0)
            return;

        int coins = coinRewards[nextMilestoneIndex];
        int exp = expRewards[nextMilestoneIndex];

        if (MoneySystem.Instance == null)
        {
            Debug.LogError("[OFFERING] MoneySystem missing!");
            return;
        }

        LevelSystem level =
            FindObjectOfType<LevelSystem>();

        if (level == null)
        {
            Debug.LogError("[OFFERING] LevelSystem missing!");
            return;
        }

        chestsReturned += amountToReturn;

        MoneySystem.Instance.money += coins;
        level.AddEXP(exp);

        Debug.Log("[OFFERING] MILESTONE COMPLETED!");
        Debug.Log("+" + coins + " COINS");
        Debug.Log("+" + exp + " EXP");

        nextMilestoneIndex++;

        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        if (nextMilestoneIndex >= milestones.Length)
        {
            if (questText != null)
            {
                questText.text =
                    "TREASURE OFFERING\n\n" +
                    "All Treasure Sigils have been offered!\n\n" +
                    "You have restored the ancient treasure collection!\n\n" +
                    "ALL MILESTONES COMPLETED!";
            }

            if (progressText != null)
                progressText.text = "100 / 100 SIGILS OFFERED";

            if (returnButton != null)
                returnButton.interactable = false;

            return;
        }

        int required = milestones[nextMilestoneIndex];

        int remaining = Mathf.Max(
            0,
            required - totalChestsCollected
        );

        if (questText != null)
        {
            questText.text =
                "TREASURE OFFERING\n\n" +

                "TREASURE SIGILS\n" +
                "Every treasure chest you discover gives you " +
                "one Treasure Sigil.\n\n" +

                "Offer your collected Sigils to receive " +
                "magical rewards!\n\n" +

                "NEXT OFFERING: " + required + " SIGILS\n" +
                "SIGILS NEEDED: " + remaining + "\n\n" +

                "REWARDS\n" +
                coinRewards[nextMilestoneIndex] + " Coins\n" +
                expRewards[nextMilestoneIndex] + " EXP\n\n" +

                "Collect more treasure chests to unlock " +
                "the next offering reward!";
        }

        if (progressText != null)
        {
            progressText.text =
                totalChestsCollected + " / " +
                required + " SIGILS COLLECTED";
        }

        if (returnButton != null)
        {
            returnButton.interactable =
                totalChestsCollected >= required;
        }
    }

    private void OnDestroy()
    {
        if (returnButton != null)
            returnButton.onClick.RemoveListener(ReturnChests);

        if (Instance == this)
            Instance = null;
    }
}