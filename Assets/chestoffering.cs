
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureOfferingQuest : MonoBehaviour
{
    public static TreasureOfferingQuest Instance;

    [Header("Quest Progress")]
    public int totalChestsCollected = 0;
    public int chestsReturned = 0;
    public int maxChests = 100;

    [Header("Milestones")]
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

    [Header("Quest UI")]
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

    // CALLED WHEN A CHEST IS COLLECTED.
    public void CollectChest()
    {
        if (totalChestsCollected >= maxChests)
            return;

        totalChestsCollected++;

        Debug.Log(
            "CHESTS: " + totalChestsCollected +
            "/" + maxChests
        );

        UpdateQuestUI();
    }

    // CALLED WHEN THE PLAYER RETURNS CHESTS.
    public void ReturnChests()
    {
        if (nextMilestoneIndex >= milestones.Length)
            return;

        int required = milestones[nextMilestoneIndex];

        if (totalChestsCollected < required)
            return;

        int amountToReturn = required - chestsReturned;

        chestsReturned += amountToReturn;

        int coins = coinRewards[nextMilestoneIndex];
        int exp = expRewards[nextMilestoneIndex];

        if (MoneySystem.Instance != null)
        {
            MoneySystem.Instance.money += coins;
        }
        else
        {
            Debug.LogError("MoneySystem not found!");
        }

        if (FindObjectOfType<LevelSystem>() != null)
        {
            FindObjectOfType<LevelSystem>().AddEXP(exp);
        }
        else
        {
            Debug.LogError("LevelSystem not found!");
        }

        Debug.Log("TREASURE MILESTONE COMPLETED!");
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
                    "THE LOST TREASURE\n\n" +
                    "You recovered every treasure chest!\n\n" +
                    "QUEST COMPLETED!";
            }

            if (progressText != null)
                progressText.text = "100 / 100 CHESTS";

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
                "THE LOST TREASURE\n\n" +
                "Help the Treasure Keeper recover his lost chests!\n\n" +
                "Next Offering: " + required + "\n" +
                "Chests Needed: " + remaining + "\n\n" +
                "REWARDS\n" +
                coinRewards[nextMilestoneIndex] + " Coins\n" +
                expRewards[nextMilestoneIndex] + " EXP";
        }

        if (progressText != null)
        {
            progressText.text =
                chestsReturned + " / " + maxChests +
                " CHESTS RETURNED";
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
