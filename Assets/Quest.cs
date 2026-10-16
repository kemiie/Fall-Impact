using UnityEngine;


public enum ItemType {
    chests,
    Apple,
    Coin
}


[CreateAssetMenu(fileName = "Quest", menuName = "NPC/Quest")]
public class Quest : ScriptableObject
{
    public string QuestName;
    public string QuestDescription;
    public ItemType QuestItem;
    public int QuestItemAmount;
    public ItemType QuestReward;
    public int QuestRewardAmount;
    public string[] QuestDialogue;
}
