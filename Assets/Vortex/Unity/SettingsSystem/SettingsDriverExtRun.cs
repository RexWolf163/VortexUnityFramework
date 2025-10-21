using UnityEngine;
using Vortex.Core.SettingsSystem.Bus;

namespace Vortex.Unity.SettingsSystem
{
    public partial class SettingsDriver
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Run() => Settings.SetDriver(Instance);
    }
}