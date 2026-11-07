using UnityEngine;

public class MagicFragment : MonoBehaviour
{
    public float rotationSpeed = 70f;
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    private Vector3 startPosition;
    private bool collected = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(
            Vector3.up * rotationSpeed * Time.deltaTime
        );

        float newY = Mathf.Sin(Time.time * floatSpeed)
                     * floatHeight;

        transform.position = startPosition
                             + Vector3.up * newY;
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            QuestManager.Instance.CollectFragment();

            Destroy(gameObject);
        }
    }
}