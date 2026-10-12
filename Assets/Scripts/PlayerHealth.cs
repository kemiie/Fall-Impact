using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts = 3;

    public Transform spawnPoint;

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;

        Debug.Log("Hearts left: " + currentHearts);

        if (currentHearts <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint.position;

        currentHearts = maxHearts;

        Debug.Log("Respawned!");
    }
}