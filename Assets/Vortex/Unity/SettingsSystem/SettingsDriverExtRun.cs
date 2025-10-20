using UnityEngine;
using Vortex.Core.SettingsSystem.Bus;

namespace Vortex.Unity.SettingsSystem
{
    public partial class SettingsDriver
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Run() => Settings.SetDriver(Instance);
    }
}