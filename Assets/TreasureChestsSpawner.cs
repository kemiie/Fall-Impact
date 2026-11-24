
using UnityEngine;
using System.Collections;

public class TreasureChestSpawner : MonoBehaviour
{
    [Header("Chest Prefab")]
    public GameObject chestPrefab;

    [Header("Initial Chest Locations")]
    public Transform[] spawnPoints;

    [Header("Bonus Chest Locations")]
    public Transform[] bonusSpawnPoints;

    [Header("Respawn Settings")]
    public float bonusRespawnTime = 60f;

    private bool bonusUnlocked = false;
    private bool[] bonusChestActive;

    private void Start()
    {
        bonusChestActive = new bool[bonusSpawnPoints.Length];

        SpawnInitialChests();
    }

    private void Update()
    {
        if (bonusUnlocked)
            return;

        if (QuestManager.Instance != null &&
            QuestManager.Instance.firstQuestCompleted &&
            QuestManager.Instance.secondQuestCompleted)
        {
            UnlockBonusChests();
        }
    }

    private void SpawnInitialChests()
    {
        if (chestPrefab == null)
        {
            Debug.LogError("Chest prefab missing!");
            return;
        }

        foreach (Transform point in spawnPoints)
        {
            if (point != null)
            {
                Instantiate(
                    chestPrefab,
                    point.position,
                    point.rotation
                );
            }
        }

        Debug.Log("Initial quest chests spawned!");
    }

    private void UnlockBonusChests()
    {
        bonusUnlocked = true;

        Debug.Log("BOTH QUESTS COMPLETED! BONUS CHESTS UNLOCKED!");

        for (int i = 0; i < bonusSpawnPoints.Length; i++)
        {
            if (bonusSpawnPoints[i] != null)
                SpawnBonusChest(i);
        }
    }

    private void SpawnBonusChest(int index)
    {
        if (chestPrefab == null ||
            index < 0 ||
            index >= bonusSpawnPoints.Length)
            return;

        Transform point = bonusSpawnPoints[index];

        if (point == null)
            return;

        GameObject chest = Instantiate(
            chestPrefab,
            point.position,
            point.rotation
        );

        TreasureChest chestScript =
            chest.GetComponent<TreasureChest>();

        if (chestScript != null)
            chestScript.Initialize(this, index);

        bonusChestActive[index] = true;
    }

    public void BonusChestCollected(int index)
    {
        if (!bonusUnlocked ||
            index < 0 ||
            index >= bonusChestActive.Length ||
            !bonusChestActive[index])
            return;

        bonusChestActive[index] = false;

        StartCoroutine(RespawnBonusChest(index));
    }

    private IEnumerator RespawnBonusChest(int index)
    {
        yield return new WaitForSeconds(bonusRespawnTime);

        SpawnBonusChest(index);
    }
}