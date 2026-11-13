
using UnityEngine;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    public int money = 0;

    public static MoneySystem Instance { get; private set; }

    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (moneyText != null)
        {
            moneyText.text = "$ " + money;
        }
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0 || money < amount)
            return false;

        money -= amount;

        return true;
    }
}