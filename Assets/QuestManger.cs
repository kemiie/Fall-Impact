
using UnityEngine;

uusing UnityEngine;

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
            Debug.LogError("Duplicate QuestManager found!");
            return;
        }

        Instance = this;
    }

    public void CollectFragment()
    {
        if (questCompleted) return;

        fragmentsCollected++;

        Debug.Log(
            "Fragments: " +
            fragmentsCollected + "/" +
            fragmentsRequired
        );

        if (fragmentsCollected >= fragmentsRequired)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        questCompleted = true;

        Debug.Log("QUEST COMPLETED!");

        if (!rewardClaimed && levelSystem != null)
        {
            rewardClaimed = true;

            levelSystem.AddEXP(rewardExperience);
            levelSystem.AddCoins(rewardCoins);

            Debug.Log("Quest rewards received!");
        }
        else if (levelSystem == null)
        {
            Debug.LogError(
                "LevelSystem is not assigned!"
            );
        }
    }
}