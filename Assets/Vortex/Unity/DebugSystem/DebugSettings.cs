using UnityEngine;
using Vortex.Unity.SettingsSystem.Storage;

namespace Vortex.Unity.DebugSystem
{
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "Settings/DebugSettings")]
    public partial class DebugSettings : SettingsStorage
    {
        [SerializeField] [ToggleButtons(singleButton: true, trueColor: "@Color.red", falseColor: "@Color.green")]
        private bool debugMode = true;

        public bool DebugMode => debugMode;
    }
}