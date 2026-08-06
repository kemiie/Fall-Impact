using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string itemName;
    public int price;

    public Sprite itemIcon;

    public InventorySlot inventorySlot;

    public void BuyItem()
    {
        if (MoneySystem.Instance.money >= price)
        {
            MoneySystem.Instance.money -= price;

            inventorySlot.AddItem(itemIcon);

            Debug.Log("Bought " + itemName);
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
}