using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public class CurrentQuest
{
    public string QuestGiverName;
    public ItemType CurrentQuestItem;
    public int questCount;
}

public class QuestManger : MonoBehaviour
{
    public CurrentQuest quest;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            NPC npc = other.GetComponent<NPC>();
            quest.QuestGiverName = npc.name;
            quest.CurrentQuestItem = npc.quest.QuestItem;
            quest.questCount = npc.quest.QuestItemAmount;
        }
    }
}
