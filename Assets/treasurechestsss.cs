using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public float interactionDistance = 4f;

    private Transform player;
    private bool collected = false;

    private TreasureChestSpawner spawner;
    private int bonusIndex = -1;

    public void Initialize(TreasureChestSpawner owner, int index)
    {
        spawner = owner;
        bonusIndex = index;
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
        else
            Debug.LogError("[CHEST] Player tag missing!");
    }

    private void Update()
    {
        if (collected || player == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        if (distance <= interactionDistance &&
            Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (collected)
            return;

        if (QuestManager.Instance == null)
            return;

        if (!QuestManager.Instance.firstQuestCompleted)
        {
            Debug.Log("Complete the crystal quest first!");
            return;
        }

        collected = true;

        if (!QuestManager.Instance.secondQuestCompleted)
        {
            QuestManager.Instance.CollectChest();
        }

        if (TreasureOfferingQuest.Instance != null)
        {
            TreasureOfferingQuest.Instance.CollectChest();
        }

        if (bonusIndex >= 0 && spawner != null)
        {
            spawner.BonusChestCollected(bonusIndex);
        }

        Debug.Log("[CHEST] Collected!");

        Destroy(gameObject);
    }
}