using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.Extensions.Abstractions
{
    public abstract class SoData : ScriptableObject
    {
#if UNITY_EDITOR
        [Button]
        private void Test() => this.PrintFields();
#endif
    }
}