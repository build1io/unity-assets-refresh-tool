#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Build1.AssetsRefreshTool.Editor
{
    [InitializeOnLoad]
    internal static class AssetsRefreshTool
    {
        public const string AutoRefreshKeyKey = "kAutoRefresh";
        public const string RefreshOnPlayKey  = "Build1_AssetsRefreshTool_RefreshOnPlayEnabled";

        static AssetsRefreshTool()
        {
            SetEnabled(GetEnabled(), true, false);
        }

        public static bool GetEnabled()
        {
            return EditorPrefs.GetBool(RefreshOnPlayKey);
        }

        public static bool SetEnabled(bool enabled, bool force = false, bool printToLog = true)
        {
            if (GetEnabled() == enabled && !force)
                return false;

            EditorPrefs.SetBool(AutoRefreshKeyKey, !enabled);
            EditorPrefs.SetBool(RefreshOnPlayKey, enabled);

            if (enabled)
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            else
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            if (printToLog)
            {
                Debug.Log(enabled
                              ? "AssetsRefreshTool: Refresh on Play enabled. Auto Refresh turned off."
                              : "AssetsRefreshTool: Refresh on Play disabled. Auto Refresh turned on.");
            }

            return true;
        }

        /*
         * Private.
         */

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode)
                return;
            Debug.Log("AssetsRefreshTool: Refreshing assets...");
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }
    }
}

#endif