
using UnityEngine;
using System.Collections.Generic;

public class TreasureChestSpawner : MonoBehaviour
{
    [Header("Chest Prefab")]
    public GameObject chestPrefab;

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;

    [Header("Settings")]
    public bool spawnOnStart = true;

    private List<GameObject> spawnedChests =
        new List<GameObject>();

    private void Start()
    {
        if (spawnOnStart)
            SpawnAllChests();
    }

    public void SpawnAllChests()
    {
        if (chestPrefab == null)
        {
            Debug.LogError("Chest prefab is missing!");
            return;
        }

        foreach (GameObject chest in spawnedChests)
        {
            if (chest != null)
                Destroy(chest);
        }

        spawnedChests.Clear();

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
                continue;

            GameObject chest = Instantiate(
                chestPrefab,
                point.position,
                point.rotation
            );

            spawnedChests.Add(chest);
        }

        Debug.Log(
            "Spawned " + spawnedChests.Count +
            " treasure chests!"
        );
    }
}