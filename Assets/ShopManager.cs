using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shopPanel;
    public Button openShopButton;
    public Button closeShopButton;
    public Button singleWishButton;
    public Button tenWishButton;

    [Header("UI Text")]
    public TMP_Text moneyText;
    public TMP_Text resultText;
    public TMP_Text pityText;

    [Header("Companions")]
    public string[] threeStarCompanions =
    {
        "Bird", "Fish", "Monkey"
    };

    public string[] fourStarCompanions =
    {
        "Lizard", "Deer", "Rat"
    };

    public string[] fiveStarCompanions =
    {
        "Octopus", "Rainbow Bird", "Snake"
    };

    [Header("Wish Prices")]
    public int singleWishPrice = 100;
    public int tenWishPrice = 1000;

    [Header("Pity Settings")]
    public int fiveStarPityLimit = 50;
    public int fourStarPityLimit = 10;

    private int wishesSinceFiveStar = 0;
    private int wishesSinceFourStar = 0;

    private void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (openShopButton != null)
            openShopButton.onClick.AddListener(OpenShop);

        if (closeShopButton != null)
            closeShopButton.onClick.AddListener(CloseShop);

        if (singleWishButton != null)
            singleWishButton.onClick.AddListener(SingleWish);

        if (tenWishButton != null)
            tenWishButton.onClick.AddListener(TenWish);

        UpdateUI();
    }

    public void OpenShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(true);

        UpdateUI();
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    private bool SpendMoney(int amount)
    {
        if (MoneySystem.Instance == null)
        {
            Debug.LogError("[SHOP] MoneySystem not found!");
            return false;
        }

        if (!MoneySystem.Instance.SpendMoney(amount))
        {
            if (resultText != null)
                resultText.text = "Not enough coins!";

            return false;
        }

        return true;
    }

    public void SingleWish()
    {
        if (!SpendMoney(singleWishPrice))
            return;

        string reward = RollReward();

        if (resultText != null)
            resultText.text = "YOUR WISH!\n\n" + reward;

        UpdateUI();
    }

    public void TenWish()
    {
        if (!SpendMoney(tenWishPrice))
            return;

        StringBuilder results = new StringBuilder();

        results.AppendLine("TEN WISHES");
        results.AppendLine();

        for (int i = 0; i < 10; i++)
        {
            results.AppendLine((i + 1) + ". " + RollReward());
            results.AppendLine();
        }

        if (resultText != null)
            resultText.text = results.ToString();

        UpdateUI();
    }

    private string RollReward()
    {
        wishesSinceFiveStar++;
        wishesSinceFourStar++;

        int rarity;
        int chance = Random.Range(0, 100);

        if (wishesSinceFiveStar >= fiveStarPityLimit)
            rarity = 5;
        else if (wishesSinceFourStar >= fourStarPityLimit)
            rarity = 4;
        else if (chance < 5)
            rarity = 5;
        else if (chance < 25)
            rarity = 4;
        else
            rarity = 3;

        if (rarity == 5)
        {
            wishesSinceFiveStar = 0;
            wishesSinceFourStar = 0;
        }
        else if (rarity == 4)
        {
            wishesSinceFourStar = 0;
        }

        string petName;

        if (rarity == 5)
            petName = GetRandom(fiveStarCompanions);
        else if (rarity == 4)
            petName = GetRandom(fourStarCompanions);
        else
            petName = GetRandom(threeStarCompanions);

        // ADD PET TO COLLECTION
        PetManager manager = PetManager.Instance;

        if (manager == null)
        {
            Debug.LogError("[SHOP] PetManager.Instance is NULL!");
        }
        else
        {
            manager.AddPet(petName, rarity);

            Debug.Log(
                "[SHOP SUCCESS] Added " + petName +
                " | Rarity: " + rarity +
                " | Total owned: " + manager.petNames.Count
            );
        }

        return new string('★', rarity) +
            " " + rarity + " STAR\n" +
            "COMPANION: " + petName;
    }

    private string GetRandom(string[] companions)
    {
        if (companions == null || companions.Length == 0)
            return "Mystery Companion";

        return companions[Random.Range(0, companions.Length)];
    }

    private void UpdateUI()
    {
        if (moneyText != null)
        {
            int balance = MoneySystem.Instance != null
                ? MoneySystem.Instance.money
                : 0;

            moneyText.text = "Coins: " + balance;
        }

        if (pityText != null)
        {
            pityText.text =
                "5 STAR PITY: " +
                wishesSinceFiveStar + " / " +
                fiveStarPityLimit +
                "\n\n4 STAR PITY: " +
                wishesSinceFourStar + " / " +
                fourStarPityLimit;
        }
    }

    private void OnDestroy()
    {
        if (openShopButton != null)
            openShopButton.onClick.RemoveListener(OpenShop);

        if (closeShopButton != null)
            closeShopButton.onClick.RemoveListener(CloseShop);

        if (singleWishButton != null)
            singleWishButton.onClick.RemoveListener(SingleWish);

        if (tenWishButton != null)
            tenWishButton.onClick.RemoveListener(TenWish);
    }
}