using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject missionPanel;
    public Button missionsButton;
    public Button closeButton;

    [Header("Mission Text")]
    public TMP_Text objectiveText;

    [Header("Quest")]
    public QuestManager questManager;

    void Start()
    {
        // Hide missions when the game starts
        missionPanel.SetActive(false);

        // Connect the buttons
        missionsButton.onClick.AddListener(ToggleMissions);
        closeButton.onClick.AddListener(CloseMissions);
    }

    void Update()
    {
        // Update the mission progress
        if (questManager != null && objectiveText != null)
        {
            if (questManager.questCompleted)
            {
                objectiveText.text =
                    "QUEST COMPLETED!\n\n" +
                    "You restored the Forgotten Meadow!";
            }
            else
            {
                objectiveText.text =
                    "OBJECTIVES\n\n" +
                    "Find magical fragments\n" +
                    "(" + questManager.fragmentsCollected +
                    "/" + questManager.fragmentsRequired + ")";
            }
        }
    }

    public void ToggleMissions()
    {
        missionPanel.SetActive(!missionPanel.activeSelf);
    }

    public void CloseMissions()
    {
        missionPanel.SetActive(false);
    }
}