using UnityEngine;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.SaveSystem
{
    public partial class SaveSystemDriver : Singleton<SaveSystemDriver>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Run() => SaveController.SetDriver(Instance);
    }
}