using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest 1 - Forgotten Meadow")]
    public int fragmentsCollected = 0;
    public int fragmentsRequired = 3;
    public bool firstQuestCompleted = false;

    [Header("Quest 2 - Hidden Treasure")]
    public int chestsCollected = 0;
    public int chestsRequired = 3;
    public bool secondQuestCompleted = false;

    [Header("Player Progression")]
    public LevelSystem levelSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CollectFragment()
    {
        if (firstQuestCompleted) return;

        fragmentsCollected++;

        Debug.Log("Fragments: " +
            fragmentsCollected + "/" + fragmentsRequired);

        if (fragmentsCollected >= fragmentsRequired)
        {
            firstQuestCompleted = true;

            Debug.Log("FORGOTTEN MEADOW COMPLETED!");

            GiveRewards(100, 50);
        }
    }

    public void CollectChest()
    {
        if (!firstQuestCompleted) return;
        if (secondQuestCompleted) return;

        chestsCollected++;

        Debug.Log("Chests: " +
            chestsCollected + "/" + chestsRequired);

        if (chestsCollected >= chestsRequired)
        {
            secondQuestCompleted = true;

            Debug.Log("HIDDEN TREASURE COMPLETED!");

            GiveRewards(250, 150);
        }
    }

    private void GiveRewards(int exp, int coins)
    {
        if (levelSystem != null)
            levelSystem.AddEXP(exp);
        else
            Debug.LogError("LevelSystem is not assigned!");

        if (MoneySystem.Instance != null)
            MoneySystem.Instance.money += coins;
        else
            Debug.LogError("MoneySystem not found!");
    }

    public string GetQuestDisplayText()
    {
        if (!firstQuestCompleted)
        {
            return "FORGOTTEN MEADOW\n\n" +
                "Collect magical fragments\n\n" +
                fragmentsCollected + " / " + fragmentsRequired +
                "\n\nReward: 100 EXP · 50 Coins";
        }

        if (!secondQuestCompleted)
        {
            return "HIDDEN TREASURE\n\n" +
                "Find the hidden treasure chests!\n\n" +
                "Chests Found: " +
                chestsCollected + " / " + chestsRequired +
                "\n\nReward: 250 EXP · 150 Coins";
        }

        return "HIDDEN TREASURE\n\n" +
            "Find the hidden treasure chests!\n\n" +
            chestsCollected + " / " + chestsRequired +
            "\n\nALL QUESTS COMPLETED!";
    }
}