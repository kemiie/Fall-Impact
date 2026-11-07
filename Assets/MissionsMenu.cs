using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject missionPanel;
    public Button missionsButton;
    public Button closeButton;

    [Header("Quest Display")]
    public TMP_Text objectiveText;

    [Header("Quest Manager")]
    public QuestManager questManager;

    private void Start()
    {
        if (missionPanel != null)
            missionPanel.SetActive(false);

        if (missionsButton != null)
            missionsButton.onClick.AddListener(ToggleMissions);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMissions);

        UpdateQuestDisplay();
    }

    private void Update()
    {
        if (missionPanel != null && missionPanel.activeSelf)
        {
            UpdateQuestDisplay();
        }
    }

    public void UpdateQuestDisplay()
    {
        if (questManager == null || objectiveText == null)
            return;

        objectiveText.text =
            questManager.GetQuestDisplayText();
    }

    public void ToggleMissions()
    {
        if (missionPanel == null) return;

        missionPanel.SetActive(!missionPanel.activeSelf);

        UpdateQuestDisplay();
    }

    public void CloseMissions()
    {
        if (missionPanel != null)
            missionPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (missionsButton != null)
            missionsButton.onClick.RemoveListener(ToggleMissions);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseMissions);
    }
}