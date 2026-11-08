using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (collected) return;

        if (QuestManager.Instance == null) return;

        if (!QuestManager.Instance.firstQuestCompleted)
            return;

        collected = true;

        QuestManager.Instance.CollectChest();

        Destroy(gameObject);
    }
}