
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    [Header("Player Progress")]
    public int totalEXP = 0;
    public int currentLevel = 1;

    [Header("Level Settings")]
    public int[] levelRequirements =
    {
        0, 300, 700, 1200, 1800,
        2500, 3300, 4200, 5200, 6300
    };

    [Header("Level Rewards")]
    public int level2Reward = 200;

    private bool level2RewardClaimed = false;

    [Header("Level Menu")]
    public GameObject levelPanel;
    public Button levelButton;
    public Button closeButton;

    [Header("Level UI")]
    public TMP_Text levelText;
    public TMP_Text expText;
    public TMP_Text nextLevelText;
    public TMP_Text currentEXPText;
    public TMP_Text requiredEXPText;
    public TMP_Text coinsText;

    public Slider expBar;

    void Start()
    {
        if (levelPanel != null)
            levelPanel.SetActive(false);

        if (levelButton != null)
            levelButton.onClick.AddListener(OpenLevelPanel);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseLevelPanel);

        if (expBar != null)
            expBar.interactable = false;

        UpdateLevel();
    }

    public void OpenLevelPanel()
    {
        if (levelPanel != null)
            levelPanel.SetActive(true);

        UpdateLevel();
    }

    public void CloseLevelPanel()
    {
        if (levelPanel != null)
            levelPanel.SetActive(false);
    }

    public void AddEXP(int amount)
    {
        if (amount <= 0) return;

        totalEXP += amount;

        UpdateLevel();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        if (MoneySystem.Instance != null)
            MoneySystem.Instance.money += amount;

        UpdateLevel();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;

        if (MoneySystem.Instance == null)
            return false;

        if (MoneySystem.Instance.money < amount)
            return false;

        MoneySystem.Instance.money -= amount;

        UpdateLevel();

        return true;
    }

    public void UpdateLevel()
    {
        if (levelRequirements == null ||
            levelRequirements.Length == 0)
            return;

        int newLevel = 1;

        for (int i = 1; i < levelRequirements.Length; i++)
        {
            if (totalEXP >= levelRequirements[i])
                newLevel = i + 1;
            else
                break;
        }

        currentLevel = newLevel;

        if (currentLevel >= 2 && !level2RewardClaimed)
        {
            level2RewardClaimed = true;

            if (MoneySystem.Instance != null)
            {
                MoneySystem.Instance.money += level2Reward;
            }

            Debug.Log("LEVEL 2 REWARD! +" +
                level2Reward + " COINS!");
        }

        int currentRequirement =
            levelRequirements[currentLevel - 1];

        bool maxLevel =
            currentLevel >= levelRequirements.Length;

        int nextRequirement = maxLevel
            ? currentRequirement
            : levelRequirements[currentLevel];

        if (levelText != null)
            levelText.text = "Level " + currentLevel;

        if (coinsText != null)
        {
            coinsText.text = MoneySystem.Instance != null
                ? MoneySystem.Instance.money.ToString()
                : "0";
        }

        if (maxLevel)
        {
            if (expText != null)
                expText.text = "MAX LEVEL";

            if (nextLevelText != null)
                nextLevelText.text = "Maximum level reached!";

            if (currentEXPText != null)
                currentEXPText.text = "MAX";

            if (requiredEXPText != null)
                requiredEXPText.text = "MAX";

            if (expBar != null)
                expBar.value = 1f;

            return;
        }

        int expIntoLevel =
            totalEXP - currentRequirement;

        int expNeeded =
            nextRequirement - currentRequirement;

        if (expText != null)
            expText.text = expIntoLevel + " / " + expNeeded + " EXP";

        if (currentEXPText != null)
            currentEXPText.text = expIntoLevel.ToString();

        if (requiredEXPText != null)
            requiredEXPText.text = expNeeded.ToString();

        if (nextLevelText != null)
            nextLevelText.text =
                (nextRequirement - totalEXP) +
                " EXP until next level";

        if (expBar != null)
            expBar.value = (float)expIntoLevel / expNeeded;
    }
}