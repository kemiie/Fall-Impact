using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    public static ExplorationManager Instance;

    [Serializable]
    public class Region
    {
        public string regionID;
        public string regionName;
        public int totalPoints = 100;
    }

    [Serializable]
    public class DiscoveryRecord
    {
        public string regionID;
        public string objectID;
        public int points;
    }

    [Serializable]
    public class SaveData
    {
        public List<DiscoveryRecord> records =
            new List<DiscoveryRecord>();
    }

    public List<Region> regions = new List<Region>();

    private List<DiscoveryRecord> discoveries =
        new List<DiscoveryRecord>();

    public event Action OnExplorationChanged;
    public bool IsDiscovered(string regionID, string objectID)
    {
        return discoveries.Exists(record =>
            record.regionID == regionID &&
            record.objectID == objectID
        );
    }

    private const string SaveKey = "ExplorationProgress";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadProgress();
    }

    public bool Discover(string regionID, string objectID, int points)
    {
        foreach (var record in discoveries)
        {
            if (record.regionID == regionID &&
                record.objectID == objectID)
                return false;
        }

        discoveries.Add(new DiscoveryRecord
        {
            regionID = regionID,
            objectID = objectID,
            points = Mathf.Max(0, points)
        });

        SaveProgress();
        OnExplorationChanged?.Invoke();

        return true;
    }

    public float GetRegionPercentage(string regionID)
    {
        Region region = regions.Find(
            r => r.regionID == regionID
        );

        if (region == null || region.totalPoints <= 0)
            return 0f;

        int earned = 0;

        foreach (var record in discoveries)
        {
            if (record.regionID == regionID)
                earned += record.points;
        }

        return Mathf.Clamp(
            earned / (float)region.totalPoints * 100f,
            0f, 100f
        );
    }

    public float GetTotalPercentage()
    {
        int possible = 0;
        int earned = 0;

        foreach (var region in regions)
        {
            possible += region.totalPoints;

            foreach (var record in discoveries)
            {
                if (record.regionID == region.regionID)
                    earned += record.points;
            }
        }

        if (possible <= 0)
            return 0f;

        return Mathf.Clamp(
            earned / (float)possible * 100f,
            0f, 100f
        );
    }

    private void SaveProgress()
    {
        SaveData data = new SaveData();
        data.records = discoveries;

        PlayerPrefs.SetString(
            SaveKey,
            JsonUtility.ToJson(data)
        );

        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return;

        SaveData data = JsonUtility.FromJson<SaveData>(
            PlayerPrefs.GetString(SaveKey)
        );

        if (data != null && data.records != null)
            discoveries = data.records;
    }
}