using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [Header("Money")]
    public int playerMoney = 100;

    [Header("Shop UI")]
    public GameObject shopPanel;

    [Header("Text")]
    public TextMeshProUGUI moneyText;

    void Start()
    {
        shopPanel.SetActive(false);

        UpdateMoneyUI();

        moneyText.text = "Shop: ";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    // OPEN SHOP
    public void OpenShop()
    {
        bool close = !shopPanel.activeSelf;
        shopPanel.SetActive(close);

        if (close)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    // BUY SWORD
    public void BuySword()
    {
        int cost = 50;

        if (playerMoney >= cost)
        {
            playerMoney -= cost;

            UpdateMoneyUI();

            //messageText.text = "Bought Sword!";
        }
        else
        {
            //messageText.text = "Not enough money!";
        }
    }

    // BUY POTION
    public void BuyPotion()
    {
        int cost = 25;

        if (playerMoney >= cost)
        {
            playerMoney -= cost;

            UpdateMoneyUI();

            //messageText.text = "Bought Potion!";
        }
        else
        {
            //messageText.text = "Not enough money!";
        }
    }

    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney;
    }
}
