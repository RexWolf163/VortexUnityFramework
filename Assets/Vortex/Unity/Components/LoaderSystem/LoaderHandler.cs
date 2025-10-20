using UnityEngine;
using Vortex.Core.LoaderSystem.Bus;

namespace Vortex.Unity.Components.LoaderSystem
{
    public class LoaderHandler : MonoBehaviour
    {
        private void OnEnable() => Loader.Run();
    }
}