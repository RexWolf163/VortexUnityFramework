using UnityEngine;
using Vortex.Core.SettingsSystem.Bus;

namespace AppScripts.Handlers
{
    public class DebugModeContainer : MonoBehaviour
    {
        private void Awake()
        {
            if (!Settings.Data().DebugMode)
                Destroy(gameObject);
        }
    }
}