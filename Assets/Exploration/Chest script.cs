using UnityEngine;

public class ChestExploration : MonoBehaviour
{
    public string regionID = "forest";
    public string chestID = "chest_001";
    public int explorationPoints = 1;

    private bool opened;

    public void OpenChest()
    {
        if (opened) return;

        opened = true;

        ExplorationManager.Instance.Discover(
            regionID,
            chestID,
            explorationPoints
        );

        Debug.Log("Chest discovered!");
    }
}