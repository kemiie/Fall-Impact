using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public enum ObjectiveType
    {
        CollectFragments,
        OpenChests,
        CollectPets,
        GetFiveStar,
        EarnCoins,
        SpendCoins
    }

    [Header("Quest Progress")]
    public int fragmentsCollected = 0;
    public int fragmentsRequired = 3;

    public int chestsCollected = 0;
    public int chestsRequired = 4;

    public bool firstQuestCompleted = false;
    public bool secondQuestCompleted = false;

    [Header("Current Quest")]
    public string currentQuestName;
    public string currentQuestDescription;

    public ObjectiveType currentObjective;

    public int currentProgress = 0;
    public int currentTarget = 3;

    [Header("Rewards")]
    public int sigils = 0;

    [Header("Level System")]
    public LevelSystem levelSystem;

    [Header("Quest UI")]
    public GameObject questPanel;
    public TMP_Text questText;

    private int questStage = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        ShopManager.StarPulled += OnStarPulled;
        ShopManager.CoinsSpent += OnCoinsSpent;
    }

    private void OnDisable()
    {
        ShopManager.StarPulled -= OnStarPulled;
        ShopManager.CoinsSpent -= OnCoinsSpent;
    }

    private void Start()
    {
        if (levelSystem == null)
            levelSystem = FindFirstObjectByType<LevelSystem>();

        if (questPanel != null)
            questPanel.SetActive(false);

        StartCrystalQuest();
        UpdateQuestUI();
    }

    // -------------------------
    // FIRST QUEST
    // -------------------------

    private void StartCrystalQuest()
    {
        questStage = 0;

        currentQuestName = "CRYSTAL COLLECTOR";
        currentQuestDescription = "Collect 3 magical fragments";

        currentObjective = ObjectiveType.CollectFragments;

        currentProgress = fragmentsCollected;
        currentTarget = fragmentsRequired;

        Debug.Log("[QUEST] Crystal Collector started!");
    }

    // -------------------------
    // SECOND QUEST
    // -------------------------

    private void StartChestQuest()
    {
        questStage = 1;

        currentQuestName = "TREASURE HUNTER";
        currentQuestDescription = "Open 4 treasure chests";

        currentObjective = ObjectiveType.OpenChests;

        currentProgress = chestsCollected;
        currentTarget = chestsRequired;

        Debug.Log("[QUEST] Treasure Hunter started!");
    }

    // -------------------------
    // RANDOM QUESTS
    // -------------------------

    private void GenerateRandomQuest()
    {
        questStage = 2;

        int random = UnityEngine.Random.Range(0, 6);

        currentProgress = 0;

        switch (random)
        {
            case 0:
                currentQuestName = "PET COLLECTOR";
                currentQuestDescription = "Collect 3 pets";
                currentObjective = ObjectiveType.CollectPets;
                currentTarget = 3;
                break;

            case 1:
                currentQuestName = "STAR COLLECTOR";
                currentQuestDescription = "Pull a 5-star companion";
                currentObjective = ObjectiveType.GetFiveStar;
                currentTarget = 1;
                break;

            case 2:
                currentQuestName = "RICH ADVENTURER";
                currentQuestDescription = "Earn 200 coins";
                currentObjective = ObjectiveType.EarnCoins;
                currentTarget = 200;
                break;

            case 3:
                currentQuestName = "LITTLE SHOPPER";
                currentQuestDescription = "Spend 300 coins";
                currentObjective = ObjectiveType.SpendCoins;
                currentTarget = 300;
                break;

            case 4:
                currentQuestName = "FRAGMENT FINDER";
                currentQuestDescription = "Collect 5 magical fragments";
                currentObjective = ObjectiveType.CollectFragments;
                currentTarget = 5;
                break;

            default:
                currentQuestName = "TREASURE ADDICT";
                currentQuestDescription = "Open 5 treasure chests";
                currentObjective = ObjectiveType.OpenChests;
                currentTarget = 5;
                break;
        }

        Debug.Log("[QUEST] New quest: " + currentQuestName);
    }

    // -------------------------
    // PROGRESS FUNCTIONS
    // -------------------------

    public void CollectFragment()
    {
        if (questStage == 0)
            fragmentsCollected++;

        AddProgress(ObjectiveType.CollectFragments, 1);
    }

    public void CollectChest()
    {
        if (questStage == 1)
            chestsCollected++;

        AddProgress(ObjectiveType.OpenChests, 1);
    }

    public void CollectPet()
    {
        AddProgress(ObjectiveType.CollectPets, 1);
    }

    public void EarnCoins(int amount)
    {
        AddProgress(ObjectiveType.EarnCoins, amount);
    }

    private void OnCoinsSpent(int amount)
    {
        AddProgress(ObjectiveType.SpendCoins, amount);
    }

    private void OnStarPulled(int rarity)
    {
        if (rarity == 5)
            AddProgress(ObjectiveType.GetFiveStar, 1);
    }

    private void AddProgress(ObjectiveType type, int amount)
    {
        if (amount <= 0)
            return;

        if (currentObjective != type)
            return;

        currentProgress = Mathf.Min(
            currentProgress + amount,
            currentTarget
        );

        UpdateQuestUI();

        if (currentProgress >= currentTarget)
            CompleteCurrentQuest();
    }

    // -------------------------
    // QUEST COMPLETION
    // -------------------------

    private void CompleteCurrentQuest()
    {
        Debug.Log("[QUEST COMPLETE] " + currentQuestName);

        GiveRewards();

        if (questStage == 0)
        {
            firstQuestCompleted = true;

            StartChestQuest();
        }
        else if (questStage == 1)
        {
            secondQuestCompleted = true;

            GenerateRandomQuest();
        }
        else
        {
            GenerateRandomQuest();
        }

        UpdateQuestUI();
    }

    private void GiveRewards()
    {
        int expReward;
        int coinReward;

        if (questStage == 0)
        {
            expReward = 100;
            coinReward = 100;
        }
        else if (questStage == 1)
        {
            expReward = 150;
            coinReward = 200;
        }
        else
        {
            expReward = UnityEngine.Random.Range(75, 201);
            coinReward = UnityEngine.Random.Range(50, 151);
        }

        if (levelSystem != null)
            levelSystem.AddEXP(expReward);
        else
            Debug.LogError("[QUEST] LevelSystem missing!");

        if (MoneySystem.Instance != null)
            MoneySystem.Instance.money += coinReward;
        else
            Debug.LogError("[QUEST] MoneySystem missing!");

        // Sigils are earned, never spent.
        if (questStage >= 1 &&
            UnityEngine.Random.Range(0, 100) < 30)
        {
            sigils++;

            Debug.Log("[QUEST] You earned a Sigil!");
        }

        Debug.Log(
            "[QUEST REWARD] +" + expReward +
            " EXP | +" + coinReward + " Coins"
        );
    }

    public void AddSigils(int amount)
    {
        if (amount > 0)
            sigils += amount;

        UpdateQuestUI();
    }

    public int GetSigils()
    {
        return sigils;
    }

    // -------------------------
    // DISPLAY
    // -------------------------

    public string GetQuestDisplayText()
    {
        return
            currentQuestName + "\n\n" +
            currentQuestDescription + "\n\n" +
            currentProgress + " / " + currentTarget +
            "\n\nSIGILS: " + sigils +
            "\n\nComplete this quest for rewards!";
    }

    public void UpdateQuestUI()
    {
        if (questText != null)
            questText.text = GetQuestDisplayText();
    }

    public void OpenQuestPanel()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.OpenPanel(questPanel);
        else if (questPanel != null)
            questPanel.SetActive(true);

        UpdateQuestUI();
    }

    public void CloseQuestPanel()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ClosePanel(questPanel);
        else if (questPanel != null)
            questPanel.SetActive(false);
    }
}