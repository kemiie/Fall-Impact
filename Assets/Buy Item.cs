using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public int price = 50;

    public void Buy()
    {
        if (MoneySystem.Instance.money >= price)
        {
            MoneySystem.Instance.money -= price;

            Debug.Log("Bought item");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}