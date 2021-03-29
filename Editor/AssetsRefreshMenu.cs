#if UNITY_EDITOR

using UnityEditor;

namespace Build1.AssetsRefreshTool.Editor
{
    [InitializeOnLoad]
    internal static class AssetsRefreshMenu
    {
        private const string AutoRefreshEnabledMenuItem = "Build1/Tools/Assets Refresh/Auto Refresh";
        private const string RefreshOnPlayMenuItem      = "Build1/Tools/Assets Refresh/Refresh on Play";
        private const string InfoMenuItem               = "Build1/Tools/Assets Refresh/Info";

        static AssetsRefreshMenu()
        {
            EditorApplication.delayCall += UpdateMenu; 
        }

        [MenuItem(AutoRefreshEnabledMenuItem, false, 1000)]
        public static void AutoRefresh()
        {
            if (AssetsRefreshTool.SetEnabled(false))
                UpdateMenu();
        }

        [MenuItem(RefreshOnPlayMenuItem, false, 1001)]
        public static void RefreshOnPlay()
        {
            if (AssetsRefreshTool.SetEnabled(true))
                UpdateMenu();
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
        
        /*
         * Private.
         */

        private static void UpdateMenu()
        {
            var enabled = AssetsRefreshTool.GetEnabled();
            Menu.SetChecked(AutoRefreshEnabledMenuItem, !enabled);
            Menu.SetChecked(RefreshOnPlayMenuItem, enabled);
        }
    }
}

#endif