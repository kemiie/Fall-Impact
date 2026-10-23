using UnityEngine;

public class COINS : MonoBehaviour
{
    public int coinValue = 10;
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (collected)
            return;

        if (MoneySystem.Instance == null)
        {
            Debug.LogError("MoneySystem instance not found!");
            return;
        }

        collected = true;

        MoneySystem.Instance.money += coinValue;

        Debug.Log("Coin collected! Money: " +
                  MoneySystem.Instance.money);

        // Make the coin disappear
        gameObject.SetActive(false);

        // Destroy it completely
        Destroy(gameObject);
    }
}