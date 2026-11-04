using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExplorationUI : MonoBehaviour
{
    public string regionID = "forest";

    public TMP_Text totalPercentageText;
    public TMP_Text regionNameText;
    public TMP_Text regionPercentageText;

    public Slider progressBar;
    public Image regionImage;

    private void OnEnable()
    {
        if (ExplorationManager.Instance != null)
        {
            ExplorationManager.Instance.OnExplorationChanged
                += RefreshUI;

            RefreshUI();
        }
    }

    private void OnDisable()
    {
        if (ExplorationManager.Instance != null)
        {
            ExplorationManager.Instance.OnExplorationChanged
                -= RefreshUI;
        }
    }

    public void RefreshUI()
    {
        var manager = ExplorationManager.Instance;

        if (manager == null) return;

        float regionPercent =
            manager.GetRegionPercentage(regionID);

        float totalPercent =
            manager.GetTotalPercentage();

        totalPercentageText.text =
            totalPercent.ToString("F1") + "%";

        regionPercentageText.text =
            regionPercent.ToString("F1") + "%";

        progressBar.value = regionPercent / 100f;

        var region = manager.regions.Find(
            r => r.regionID == regionID
        );

        if (region != null)
            regionNameText.text = region.regionName;
    }
}
