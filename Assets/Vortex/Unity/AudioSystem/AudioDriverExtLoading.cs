using UnityEngine;
using Vortex.Core.AudioSystem.Bus;

namespace Vortex.Unity.AudioSystem
{
    public partial class AudioDriver
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Run()
        {
            AudioProvider.SetDriver(Instance);
        }
    }
}