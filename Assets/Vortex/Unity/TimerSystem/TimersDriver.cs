using UnityEngine;
using Vortex.Core.System.Abstractions;
using Vortex.Core.TimerSystem;
using Vortex.Core.TimerSystem.Bus;

namespace Vortex.Unity.TimerSystem
{
    public partial class TimersDriver : Singleton<TimersDriver>, ITimersDriver
    {
        public void Init()
        {
            //Ignore
        }

        public void Destroy()
        {
            //Ignore
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Register()
        {
            Timers.SetDriver(Instance);
        }
    }
}