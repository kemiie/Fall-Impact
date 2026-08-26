using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;

    public List<QuestObjective> objectives = new List<QuestObjective>();

    private void OnValidate()
    {
        // Generate a unique ID if one doesn't already exist
        if (string.IsNullOrEmpty(questID))
        {
            questID = Guid.NewGuid().ToString();
        }

        // Make sure the objective list isn't null
        if (objectives == null)
        {
            objectives = new List<QuestObjective>();
        }

        // Generate IDs for objectives that don't have one
        foreach (var objective in objectives)
        {
            if (objective != null && string.IsNullOrEmpty(objective.objectiveID))
            {
                objective.objectiveID = Guid.NewGuid().ToString();
            }

            if (objective != null)
            {
                objective.requiredAmount = Mathf.Max(1, objective.requiredAmount);
            }
        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveID;
    public string description;
    public ObjectiveType type;
    public int requiredAmount = 1;
    public int currentAmount;

    public bool IsCompleted => currentAmount >= requiredAmount;
}

public enum ObjectiveType
{
    CollectItem,
    DefeatEnemy,
    ReachLocation,
    TalkNPC,
    Custom
}

[System.Serializable]
public class QuestProgress
{
    public Quest quest;
    public List<QuestObjective> objectives = new List<QuestObjective>();

    public QuestProgress(Quest quest)
    {
        this.quest = quest;

        if (quest == null || quest.objectives == null)
        {
            return;
        }

        // Deep copy objectives so runtime progress
        // doesn't modify the original Quest asset.
        foreach (var obj in quest.objectives)
        {
            if (obj == null)
                continue;

            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });
        }
    }

    public bool IsCompleted => objectives.TrueForAll(o => o.IsCompleted);

    public string QuestID => quest != null ? quest.questID : string.Empty;
}