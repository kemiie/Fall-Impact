using System.Collections;
using UnityEngine;
using TMPro;

public class TreasureChest : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public float interactionDistance = 4f;
    public Transform player;

    [Header("Chest Visual")]
    public GameObject chestVisual;
    public GameObject promptObject;
    public TMP_Text promptText;

    [Header("Respawn")]
    public float respawnTime = 30f;

    [Header("Rewards")]
    public int minCoins = 10;
    public int maxCoins = 50;
    public int minEXP = 10;
    public int maxEXP = 40;

    [Header("Level System")]
    public LevelSystem levelSystem;

    private bool opened;
    private bool playerNearby;

    private void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer =
                GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (levelSystem == null)
            levelSystem = FindFirstObjectByType<LevelSystem>();

        if (promptObject != null)
            promptObject.SetActive(false);
    }

    private void Update()
    {
        if (opened || player == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        playerNearby = distance <= interactionDistance;

        if (promptObject != null)
            promptObject.SetActive(playerNearby);

        if (promptText != null && playerNearby)
            promptText.text = "PRESS E TO OPEN CHEST";

        if (!playerNearby)
            return;

        if (UIManager.Instance != null &&
            UIManager.Instance.IsPanelOpen())
            return;

        if (Input.GetKeyDown(interactKey))
            OpenChest();
    }

    public void OpenChest()
    {
        if (opened)
            return;

        opened = true;

        if (promptObject != null)
            promptObject.SetActive(false);

        if (chestVisual != null)
            chestVisual.SetActive(false);

        GiveRewards();

        StartCoroutine(RespawnChest());
    }

    private void GiveRewards()
    {
        int coins = Random.Range(minCoins, maxCoins + 1);
        int exp = Random.Range(minEXP, maxEXP + 1);

        if (MoneySystem.Instance != null)
        {
            MoneySystem.Instance.money += coins;

            if (QuestManager.Instance != null)
                QuestManager.Instance.EarnCoins(coins);
        }

        if (levelSystem != null)
            levelSystem.AddEXP(exp);

        if (QuestManager.Instance != null)
            QuestManager.Instance.CollectChest();

        Debug.Log(
            "[CHEST OPENED] +" + coins +
            " Coins | +" + exp + " EXP"
        );
    }

    private IEnumerator RespawnChest()
    {
        yield return new WaitForSeconds(respawnTime);

        if (chestVisual != null)
            chestVisual.SetActive(true);

        opened = false;

        Debug.Log("[CHEST] Respawned!");
    }
}