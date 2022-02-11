#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Build1.UnityAssetsRefreshTool.Editor
{
    [InitializeOnLoad]
    internal static class AssetsRefreshTool
    {
        public const string AutoRefreshKeyKey  = "kAutoRefresh";
        public const string RefreshOnPlayKey   = "Build1_AssetsRefreshTool_RefreshOnPlayEnabled";
        public const string AppleSiliconFixKey = "Build1_AssetsRefreshTool_AppleSiliconFixEnabled";

        static AssetsRefreshTool()
        {
            SetRefreshOnPlayEnabled(GetRefreshOnPlayEnabled(), true, false);
            SetAppleSiliconFixEnabled(GetAppleSiliconFixEnabled(), true, false);
            
            UnityEditorFocusTool.OnFocus += OnEditorFocusChanged;
        }

        /*
         * Auto Refresh.
         */

        public static bool GetAutoRefreshEnabled()
        {
            return EditorPrefs.GetBool(AutoRefreshKeyKey);
        }
        
        public static bool DisableAutoRefresh()
        {
            if (!EditorPrefs.GetBool(AutoRefreshKeyKey))
                return false;

            EditorPrefs.SetBool(AutoRefreshKeyKey, false);
            return true;
        }

        public static bool EnableAutoRefresh()
        {
            if (EditorPrefs.GetBool(AutoRefreshKeyKey))
                return false;

            EditorPrefs.SetBool(AutoRefreshKeyKey, true);
            return true;
        }

        /*
         * Refresh On Play.
         */

        public static bool GetRefreshOnPlayEnabled()
        {
            return EditorPrefs.GetBool(RefreshOnPlayKey);
        }

        public static bool SetRefreshOnPlayEnabled(bool enabled, bool force, bool printToLog)
        {
            if (GetRefreshOnPlayEnabled() == enabled && !force)
                return false;

            EditorPrefs.SetBool(RefreshOnPlayKey, enabled);

            if (enabled)
            {
                DisableAutoRefresh();
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }
            else
            {
                EnableAutoRefresh();
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                AssetDatabase.Refresh(ImportAssetOptions.Default);
            }

            if (printToLog)
            {
                Debug.Log(enabled
                              ? "AssetsRefreshTool: Refresh on Play enabled. Auto Refresh turned off."
                              : "AssetsRefreshTool: Refresh on Play disabled. Auto Refresh turned on.");
            }

            return true;
        }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode)
                return;
            Debug.Log("AssetsRefreshTool: Refreshing assets...");
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }

        /*
         * Apple Silicon Fix.
         */

        public static bool GetAppleSiliconFixEnabled()
        {
            return EditorPrefs.GetBool(AppleSiliconFixKey);
        }

        public static bool SetAppleSiliconFixEnabled(bool enabled, bool force, bool printToLog)
        {
            if (GetAppleSiliconFixEnabled() == enabled && !force && !CheckIsAppleSilicon())
                return false;
            
            EditorPrefs.SetBool(AppleSiliconFixKey, enabled);

            if (!enabled)
                SetRefreshOnPlayEnabled(GetRefreshOnPlayEnabled(), true, false);

            if (printToLog)
            {
                Debug.Log(enabled
                              ? "AssetsRefreshTool: Apple Silicon Fix enabled."
                              : "AssetsRefreshTool: Apple Silicon Fix disabled.");
            }
            
            return true;
        }

        private static void OnEditorFocusChanged(bool focused)
        {
            if (!GetAppleSiliconFixEnabled() || Application.isPlaying)
                return;
            
            if (focused)
            {
                if (GetAutoRefreshEnabled())
                    return;
                
                EnableAutoRefresh();
                AssetDatabase.Refresh();
            }
            else
            {
                DisableAutoRefresh();
            }
        }

        private static bool CheckIsAppleSilicon()
        {
            Debug.Log(SystemInfo.processorType);
            return true;
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (GetAppleSiliconFixEnabled())
                Debug.Log("AssetsRefreshTool: Scripts reloaded");
        }
    }
}

#endif