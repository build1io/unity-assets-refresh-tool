using System;
using UnityEditor;

namespace Build1.UnityAssetsRefreshTool.Editor
{
    [InitializeOnLoad]
    internal static class UnityEditorFocusTool
    {
        public static event Action<bool> OnFocus;
        
        private static bool _appFocused;

        static UnityEditorFocusTool()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (!_appFocused && UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnFocus?.Invoke(true);
            }
            else if (_appFocused && !UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnFocus?.Invoke(false);
            }
        }
    }
}