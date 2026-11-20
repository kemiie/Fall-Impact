
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Panel")]
    public GameObject tutorialPanel;

    [Header("Tutorial Text")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text pageText;

    [Header("Buttons")]
    public Button continueButton;
    public Button skipButton;

    private int currentPage = 0;

    private string[] titles =
    {
        "WELCOME, ADVENTURER!",
        "COMPLETE QUESTS",
        "COLLECT PETS",
        "EXPLORE FOR TREASURE"
    };

    private string[] descriptions =
    {
        "Welcome to your adventure! Explore the world, complete quests, collect pets, and become stronger!",

        "Meet NPCs around the world and complete their quests. Earn coins and EXP to level up!",

        "Use your coins to summon pets. Your five strongest pets will automatically follow you. All your pets generate passive income!",

        "Search the world for hidden treasure chests! Collect them and return to the Treasure Keeper to exchange them for coins and EXP!"
    };

    private void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(NextPage);

        if (skipButton != null)
            skipButton.onClick.AddListener(CloseTutorial);

        // Only show once.
        if (!PlayerPrefs.HasKey("TutorialCompleted"))
        {
            OpenTutorial();
        }
    }

    public void OpenTutorial()
    {
        currentPage = 0;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        Time.timeScale = 0f;

        UpdatePage();
    }

    public void NextPage()
    {
        currentPage++;

        if (currentPage >= titles.Length)
        {
            CloseTutorial();
            return;
        }

        UpdatePage();
    }

    private void UpdatePage()
    {
        if (titleText != null)
            titleText.text = titles[currentPage];

        if (descriptionText != null)
            descriptionText.text = descriptions[currentPage];

        if (pageText != null)
            pageText.text =
                "PAGE " + (currentPage + 1) +
                " / " + titles.Length;

        if (continueButton != null)
        {
            TMP_Text buttonText =
                continueButton.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text =
                    currentPage == titles.Length - 1
                    ? "START ADVENTURE"
                    : "CONTINUE";
            }
        }
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;

        if (continueButton != null)
            continueButton.onClick.RemoveListener(NextPage);

        if (skipButton != null)
            skipButton.onClick.RemoveListener(CloseTutorial);
    }
}