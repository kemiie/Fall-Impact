using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    [Header("Tutorial Menu")]
    public GameObject tutorialPanel;
    public Button closeButton;

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(ClosePanel);
            closeButton.onClick.AddListener(ClosePanel);
        }

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void OpenPanel()
    {
        if (tutorialPanel == null)
            return;

        tutorialPanel.SetActive(true);

        if (CursorLock.Instance != null)
            CursorLock.Instance.UnlockCursor();
    }

    public void ClosePanel()
    {
        if (tutorialPanel == null)
            return;

        tutorialPanel.SetActive(false);

        if (UIManager.Instance != null &&
            UIManager.Instance.IsPanelOpen())
            return;

        if (CursorLock.Instance != null)
            CursorLock.Instance.LockCursor();
    }

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(ClosePanel);
    }
}