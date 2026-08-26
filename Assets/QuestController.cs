using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    public List<QuestProgress> activeQuests = new();

    private QuestUI questUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        questUI = FindObjectOfType<QuestUI>();
    }

    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questID))
            return;

        activeQuests.Add(new QuestProgress(quest));

        questUI.UpdateQuest();
    }

    public bool IsQuestActive(string questID)
    {
        return activeQuests.Exists(q => q.QuestID == questID);
    }
}