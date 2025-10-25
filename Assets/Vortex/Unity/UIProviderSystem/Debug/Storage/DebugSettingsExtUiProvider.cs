using UnityEngine;

namespace Vortex.Unity.DebugSystem
{
    public partial class DebugSettings
    {
        [SerializeField]
#if UNITY_EDITOR
        [ToggleButtons(singleButton: true, trueColor: "@Color.red", falseColor: "@Color.green")]
#endif
        private bool uiLogs;

        public bool UiDebugMode => DebugMode && uiLogs;
    }
}