
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    [Header("Player Progress")]
    public int totalEXP = 0;
    public int totalCoins = 0;
    public int currentLevel = 1;

    [Header("Level Settings")]
    public int[] levelRequirements =
    {
        0, 300, 700, 1200, 1800,
        2500, 3300, 4200, 5200, 6300
    };

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

        totalCoins += amount;

        UpdateLevel();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || totalCoins < amount)
            return false;

        totalCoins -= amount;

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

        if (newLevel > currentLevel)
        {
            Debug.Log("LEVEL UP! Level " + newLevel);
        }

        currentLevel = newLevel;

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
            coinsText.text = totalCoins.ToString();

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
        {
            expText.text =
                expIntoLevel + " / " + expNeeded + " EXP";
        }

        if (currentEXPText != null)
            currentEXPText.text = expIntoLevel.ToString();

        if (requiredEXPText != null)
            requiredEXPText.text = expNeeded.ToString();

        if (nextLevelText != null)
        {
            nextLevelText.text =
                (nextRequirement - totalEXP) +
                " EXP until next level";
        }

        if (expBar != null)
        {
            expBar.value =
                (float)expIntoLevel / expNeeded;
        }
    }
}