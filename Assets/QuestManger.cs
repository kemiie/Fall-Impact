

using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest Progress")]
    public int fragmentsCollected = 0;
    public int fragmentsRequired = 3;
    public bool questCompleted = false;

    [Header("Quest Rewards")]
    public int rewardExperience = 100;
    public int rewardCoins = 50;

    [Header("Player Progression")]
    public LevelSystem levelSystem;

    private bool rewardClaimed = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate QuestManager!");
            return;
        }

        Instance = this;
    }

    public void CollectFragment()
    {
        if (questCompleted) return;

        fragmentsCollected++;

        Debug.Log("Fragments: " +
            fragmentsCollected + "/" +
            fragmentsRequired);

        if (fragmentsCollected >= fragmentsRequired)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        questCompleted = true;

        Debug.Log("QUEST COMPLETED!");

        GiveRewards();
    }

    private void GiveRewards()
    {
        if (rewardClaimed) return;

        if (levelSystem == null)
        {
            Debug.LogError("LevelSystem is not assigned!");
            return;
        }

        levelSystem.AddEXP(rewardExperience);
        levelSystem.AddCoins(rewardCoins);

        rewardClaimed = true;

        Debug.Log("REWARDS GIVEN: +" +
            rewardExperience + " EXP, +" +
            rewardCoins + " Coins!");
    }

    public string GetQuestDisplayText()
    {
        if (questCompleted)
        {
            return "FORGOTTEN MEADOW\n\n" +
                "Collect magical fragments\n\n" +
                fragmentsCollected + " / " + fragmentsRequired +
                "\n\nQUEST COMPLETED!\n" +
                "Rewards automatically received!";
        }

        return "FORGOTTEN MEADOW\n\n" +
            "Collect magical fragments\n\n" +
            fragmentsCollected + " / " + fragmentsRequired +
            "\n\nReward: " + rewardExperience +
            " EXP · " + rewardCoins + " Coins";
    }
}