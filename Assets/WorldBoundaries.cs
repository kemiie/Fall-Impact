using UnityEngine;

public class WorldBoundary : MonoBehaviour
{
    [Header("Map Center")]
    public Transform mapCenter;

    [Header("Map Size")]
    public float mapWidth = 100f;
    public float mapDepth = 100f;

    [Header("Wall Settings")]
    public float wallHeight = 20f;
    public float wallThickness = 3f;

    private void Start()
    {
        if (mapCenter == null)
        {
            Debug.LogError(
                "[BOUNDARY] Please assign your Map Center!"
            );
            return;
        }

        CreateWall(
            "North Border",
            new Vector3(0, wallHeight / 2, mapDepth / 2),
            new Vector3(mapWidth, wallHeight, wallThickness)
        );

        CreateWall(
            "South Border",
            new Vector3(0, wallHeight / 2, -mapDepth / 2),
            new Vector3(mapWidth, wallHeight, wallThickness)
        );

        CreateWall(
            "East Border",
            new Vector3(mapWidth / 2, wallHeight / 2, 0),
            new Vector3(wallThickness, wallHeight, mapDepth)
        );

        CreateWall(
            "West Border",
            new Vector3(-mapWidth / 2, wallHeight / 2, 0),
            new Vector3(wallThickness, wallHeight, mapDepth)
        );

        Debug.Log("[BOUNDARY] Four walls created!");
    }

    private void CreateWall(
        string wallName,
        Vector3 offset,
        Vector3 size)
    {
        GameObject wall = new GameObject(wallName);

        wall.transform.SetParent(transform);

        wall.transform.position =
            mapCenter.position + offset;

        BoxCollider box = wall.AddComponent<BoxCollider>();

        box.size = size;
    }
}