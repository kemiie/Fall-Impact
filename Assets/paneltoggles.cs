using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    [Header("Tutorial Menu")]
    public GameObject tutorialPanel;
    public Button tutorialButton;
    public Button closeButton;

    private void Start()
    {
        tutorialButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        tutorialPanel.SetActive(false);
    }
}