using UnityEngine;

public class Pet : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float stopDistance = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > stopDistance)
        {
            // Move current position towards target at 'speed' units per second
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
