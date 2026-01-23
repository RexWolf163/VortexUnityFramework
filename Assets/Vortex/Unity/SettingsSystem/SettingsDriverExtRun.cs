#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Vortex.Core.SettingsSystem.Bus;

namespace Vortex.Unity.SettingsSystem
{
    public partial class SettingsDriver
    {
        [InitializeOnLoadMethod]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Run()
        {
            if (Settings.SetDriver(Instance))
            {
                if (Settings.HasDriver() && Application.isPlaying)
                    Debug.LogWarning(
                        "[SettingsDriver] не удалось задать драйвер для сервиса Settings. Драйвер уже установлен");
                else
                    Debug.LogWarning("[SettingsDriver] не удалось задать драйвер для сервиса Settings");
            }
        }
    }
}
#endif