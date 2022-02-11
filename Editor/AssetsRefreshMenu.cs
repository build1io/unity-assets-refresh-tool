#if UNITY_EDITOR

using UnityEditor;

namespace Build1.UnityAssetsRefreshTool.Editor
{
    [InitializeOnLoad]
    internal static class AssetsRefreshMenu
    {
        private const string AutoRefreshEnabledMenuItem = "Tools/Build1/Assets Refresh Tool/Auto Refresh/Enable";
        private const string RefreshOnPlayMenuItem      = "Tools/Build1/Assets Refresh Tool/Auto Refresh/Refresh on Play";
        private const string InfoMenuItem               = "Tools/Build1/Assets Refresh Tool/Auto Refresh/Info";

        private const string AppleSiliconFixEnabledMenuItem  = "Tools/Build1/Assets Refresh Tool/Apple Silicon Fix/Enable";
        private const string AppleSiliconFixDisabledMenuItem = "Tools/Build1/Assets Refresh Tool/Apple Silicon Fix/Disable";
        private const string AppleSiliconFixInfoMenuItem     = "Tools/Build1/Assets Refresh Tool/Apple Silicon Fix/Info";

        static AssetsRefreshMenu()
        {
            EditorApplication.delayCall += UpdateMenu;
        }

        [MenuItem(AutoRefreshEnabledMenuItem, false, 1000)]
        public static void AutoRefresh()
        {
            if (AssetsRefreshTool.SetRefreshOnPlayEnabled(false, false, true))
                UpdateMenu();
        }
        
        [MenuItem(AutoRefreshEnabledMenuItem, true, 1000)]
        public static bool AutoRefreshValidation()
        {
            return AssetsRefreshTool.GetRefreshOnPlayEnabled() && !AssetsRefreshTool.GetAppleSiliconFixEnabled();
        }

        [MenuItem(RefreshOnPlayMenuItem, false, 1001)]
        public static void RefreshOnPlay()
        {
            if (AssetsRefreshTool.SetRefreshOnPlayEnabled(true, false, true))
                UpdateMenu();
        }
        
        [MenuItem(RefreshOnPlayMenuItem, true, 1001)]
        public static bool RefreshOnPlayValidation()
        {
            return !AssetsRefreshTool.GetRefreshOnPlayEnabled() && !AssetsRefreshTool.GetAppleSiliconFixEnabled();
        }

        [MenuItem(InfoMenuItem, false, 1050)]
        public static void Info()
        {
            EditorUtility.DisplayDialog("Assets Refresh Tool",
                                        "This is a short instruction for Assets Refresh Tool.\n\n" +
                                        "Auto Refresh - is the default Unity's Auto Refresh feature.\n\n" +
                                        "Refresh on Play - disables the Auto Refresh and performs refresh on play.\n\n" +
                                        "With Refresh on Play enabled Unity Editor will not update project assets/metadata. You'll have to refresh it manually by using your OS's hot key.\n\n" +
                                        "Refresh on Play saves a lot of time when you work mostly on the code. Code changes and window switching will not stop play and will not trigger scripts recompilation when you don't need it.\n\n" +
                                        "Author: Vasyl Horbachenko",
                                        "Got it!");
        }

        [MenuItem(AppleSiliconFixEnabledMenuItem, false, 1002)]
        public static void AppleSiliconFixEnabled()
        {
            if (AssetsRefreshTool.SetAppleSiliconFixEnabled(true, false, true))
                UpdateMenu();
        }
        
        [MenuItem(AppleSiliconFixEnabledMenuItem, true, 1002)]
        public static bool AppleSiliconFixEnabledValidation()
        {
            return !AssetsRefreshTool.GetAppleSiliconFixEnabled();
        }
        
        [MenuItem(AppleSiliconFixDisabledMenuItem, false, 1003)]
        public static void AppleSiliconFixDisabled()
        {
            if (AssetsRefreshTool.SetAppleSiliconFixEnabled(false, false, true))
                UpdateMenu();
        }
        
        [MenuItem(AppleSiliconFixDisabledMenuItem, true, 1003)]
        public static bool AppleSiliconFixDisabledValidation()
        {
            return AssetsRefreshTool.GetAppleSiliconFixEnabled();
        }
        
        [MenuItem(AppleSiliconFixInfoMenuItem, false, 1053)]
        public static void AppleSiliconFixInfo()
        {
            EditorUtility.DisplayDialog("Apple Silicon Fix",
                                        "This is a short instruction for Apple Silicon Fix.\n\n" +
                                        "When enabled, Auto Refresh is disable every time Unity Editor loses focus.\n" +
                                        "When focused, Auto Refresh is enabled and assets refresh is requested.\n\n" +
                                        "Editor still crashes sometimes, but not during scripts editing.\n\n" +
                                        "We hope it'll get better soon." + "\n" + 
                                        "Good luck! =)",
                                        "Got it!");
        }

        /*
         * Private.
         */

        private static void UpdateMenu()
        {
            var enabled = AssetsRefreshTool.GetRefreshOnPlayEnabled();
            Menu.SetChecked(AutoRefreshEnabledMenuItem, !enabled);
            Menu.SetChecked(RefreshOnPlayMenuItem, enabled);

            enabled = AssetsRefreshTool.GetAppleSiliconFixEnabled();
            Menu.SetChecked(AppleSiliconFixEnabledMenuItem, enabled);
            Menu.SetChecked(AppleSiliconFixDisabledMenuItem, !enabled);
        }
    }
}

#endif