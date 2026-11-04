using UnityEngine;

public class ExplorationSystem : MonoBehaviour
{
    [Header("Exploration UI")]
    public GameObject explorationPanel;

    void Start()
    {
        explorationPanel.SetActive(false);
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

    // OPEN / CLOSE EXPLORATION
    public void OpenExplorationPanel()
    {
        bool close = !explorationPanel.activeSelf;
        explorationPanel.SetActive(close);

        if (close)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}