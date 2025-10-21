using UnityEditor;
using Vortex.Core.TimerSystem.Bus;

namespace Vortex.Unity.TimerSystem
{
    public partial class TimersDriver
    {
        [InitializeOnLoadMethod]
        private static void EditorRegister()
        {
            Timers.SetDriver(Instance);
        }
    }
}