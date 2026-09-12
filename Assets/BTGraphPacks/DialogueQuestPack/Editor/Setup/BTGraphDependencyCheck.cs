using System;
using UnityEditor;
using UnityEngine;

namespace DialoguePack.Editor.Setup
{
    /// <summary>
    /// Verifies that the BTGraph core asset is present in the project.
    /// This pack is an extension of BTGraph and cannot be used on its own.
    /// This assembly intentionally has no references, so the check still
    /// runs (and explains the problem) when the rest of the pack fails to compile.
    /// </summary>
    [InitializeOnLoad]
    internal static class BTGraphDependencyCheck
    {
        private const string PackName = "Dialogue & Quest Pack for BTGraph";
        private const string SessionKey = "BTGraph.DialogueQuest.Setup.DependencyWarningShown";

        static BTGraphDependencyCheck()
        {
            EditorApplication.delayCall += Validate;
        }

        private static void Validate()
        {
            if (Type.GetType("BTs.Runtime.BTAsset, BTGraph.Shared.Runtime") != null)
                return;

            Debug.LogError(
                PackName + " requires the BTGraph core asset, which was not found in this project. " +
                "Import BTGraph (Basic or Pro) from the Unity Asset Store, then reimport this pack. " +
                "Until BTGraph is installed, the scripts in this pack cannot compile.");

            if (Application.isBatchMode || SessionState.GetBool(SessionKey, false))
                return;

            SessionState.SetBool(SessionKey, true);
            EditorUtility.DisplayDialog(
                "BTGraph required",
                PackName + " is an extension pack for BTGraph and requires the BTGraph core asset. " +
                "Please import BTGraph (Basic or Pro) from the Unity Asset Store, then reimport this pack.",
                "OK");
        }
    }
}
