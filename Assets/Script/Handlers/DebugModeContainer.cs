using UnityEngine;
using Vortex.Core.SettingsSystem.Bus;

namespace Script.Handlers
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