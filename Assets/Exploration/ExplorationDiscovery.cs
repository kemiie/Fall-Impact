using UnityEngine;

public class ExplorationDiscoverable : MonoBehaviour
{
    [Header("Discovery Settings")]
    public string regionID = "forest";
    public string objectID = "discovery_001";
    public int explorationPoints = 1;

    public bool IsDiscovered { get; private set; }

    private void Start()
    {
        if (ExplorationManager.Instance == null)
            return;

        IsDiscovered =
            ExplorationManager.Instance.IsDiscovered(
                regionID,
                objectID
            );
    }

    public void Discover()
    {
        if (IsDiscovered)
            return;

        if (ExplorationManager.Instance == null)
        {
            Debug.LogWarning("ExplorationManager not found!");
            return;
        }

        bool success = ExplorationManager.Instance.Discover(
            regionID,
            objectID,
            explorationPoints
        );

        if (success)
        {
            IsDiscovered = true;
            Debug.Log("Exploration object discovered: " + objectID);
        }
    }
}