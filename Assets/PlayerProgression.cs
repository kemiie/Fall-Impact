using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerProgression : MonoBehaviour
{
    [Header("Player Stats")]
    public int level = 1;
    public int experience = 0;
    public int coins = 0;

    [Header("Level Settings")]
    public int baseExperienceRequired = 100;

    [Header("UI Text References")]
    public TMP_Text levelText;
    public TMP_Text experienceText;
    public TMP_Text coinsText;
    public TMP_Text notificationText;
    public TMP_Text explorationText;
    private Coroutine notificationRoutine;

    private void Start()
    {
        if (notificationText != null)
            notificationText.text = "";
    }

    public void AwardReward(int exp, int coinReward)
    {
        experience += exp;
        coins += coinReward;

        ShowNotification(
            "+" + exp + " EXP  +" +
            coinReward + " Coins!"
        );

        while (experience >= ExperienceRequired())
        {
            experience -= ExperienceRequired();
            level++;

            ShowNotification("LEVEL UP! Level " + level);
        }
    }

    public int ExperienceRequired()
    {
        return baseExperienceRequired +
               (level - 1) * 50;
    }

    private void ShowNotification(string message)
    {
        if (notificationText == null) return;

        if (notificationRoutine != null)
            StopCoroutine(notificationRoutine);

        notificationRoutine =
            StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message;

        yield return new WaitForSeconds(3f);

        notificationText.text = "";
        notificationRoutine = null;
    }
}