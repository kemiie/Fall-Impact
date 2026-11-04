using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI")]
    public TMP_Text interactionPrompt;

    private ChestExploration nearbyChest;

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (nearbyChest == null)
            return;

        if (Input.GetKeyDown(interactionKey))
        {
            nearbyChest.OpenChest();

            if (nearbyChest.IsOpened)
            {
                if (interactionPrompt != null)
                    interactionPrompt.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ChestExploration chest =
            other.GetComponentInParent<ChestExploration>();

        if (chest == null || chest.IsOpened)
            return;

        nearbyChest = chest;

        if (interactionPrompt != null)
        {
            interactionPrompt.text = "E - Open";
            interactionPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ChestExploration chest =
            other.GetComponentInParent<ChestExploration>();

        if (chest == nearbyChest)
        {
            nearbyChest = null;

            if (interactionPrompt != null)
                interactionPrompt.gameObject.SetActive(false);
        }
    }
}

