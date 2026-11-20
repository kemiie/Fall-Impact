using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [Header("Chest Settings")]
    public float interactionDistance = 4f;

    private Transform player;
    private bool collected = false;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
        else
            Debug.LogError("Player tag not found!");
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

        if (TreasureOfferingQuest.Instance == null)
        {
            Debug.LogError(
                "TreasureOfferingQuest is missing!"
            );
            return;
        }

        collected = true;

        TreasureOfferingQuest.Instance.CollectChest();

        Debug.Log("CHEST COLLECTED!");

        Destroy(gameObject);
    }
}