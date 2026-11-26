using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Register Every Panel Here")]
    public List<GameObject> allPanels = new List<GameObject>();

    private GameObject currentPanel;

    public GameObject CurrentPanel => currentPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CloseAllPanels();
    }

    public void OpenPanel(GameObject panel)
    {
        if (panel == null)
            return;

        // Close every other registered panel.
        foreach (GameObject other in allPanels)
        {
            if (other != null && other != panel)
                other.SetActive(false);
        }

        // Close previous panel.
        if (currentPanel != null && currentPanel != panel)
            currentPanel.SetActive(false);

        currentPanel = panel;
        currentPanel.SetActive(true);

        if (CursorLock.Instance != null)
            CursorLock.Instance.UnlockCursor();

        Debug.Log("[UI] Opened: " + panel.name);
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel == null)
            return;

        panel.SetActive(false);

        if (currentPanel == panel)
            currentPanel = null;

        if (currentPanel == null)
        {
            if (CursorLock.Instance != null)
                CursorLock.Instance.LockCursor();
        }
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel == null)
            return;

        currentPanel.SetActive(false);
        currentPanel = null;

        if (CursorLock.Instance != null)
            CursorLock.Instance.LockCursor();
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        currentPanel = null;
    }

    public void TogglePanel(GameObject panel)
    {
        if (panel == null)
            return;

        if (currentPanel == panel)
            ClosePanel(panel);
        else
            OpenPanel(panel);
    }

    public bool IsPanelOpen()
    {
        return currentPanel != null &&
               currentPanel.activeSelf;
    }
}