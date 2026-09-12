using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IPBR_EnvironmentWarning : MonoBehaviour
{
    [SerializeField] private string folderPath = "assets/BOTD_Standard Shader";
    [SerializeField] private string folderPathURP =  "assets/Book of the Dead - URP";
    public GameObject panel;
    #if UNITY_EDITOR
    
    private void Awake()
    {
        if (AssetDatabase.IsValidFolder(folderPath) || AssetDatabase.IsValidFolder(folderPathURP))
        {
            gameObject.SetActive(false);
            return;
        }
        
        panel.SetActive(true);
    }

    public void GoToAssetStore() => Application.OpenURL("https://assetstore.unity.com/packages/book-of-the-dead-urp/369748?aid=1100lxWw");

    #endif
}
