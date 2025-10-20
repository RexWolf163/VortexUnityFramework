using UnityEngine;

namespace Vortex.Unity.DebugSystem
{
    public partial class DebugSettings
    {
        [SerializeField] [ToggleButtons(singleButton: true, trueColor: "@Color.red", falseColor: "@Color.green")]
        private bool appStates;

        public bool AppStateDebugMode => appStates;
    }
}