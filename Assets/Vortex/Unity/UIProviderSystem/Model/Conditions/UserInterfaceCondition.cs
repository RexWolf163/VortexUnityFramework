using System;
using Sirenix.OdinInspector;

namespace Vortex.Unity.UIProviderSystem.Model.Conditions
{
    [Serializable]
    public abstract class UserInterfaceCondition
    {
        [DisplayAsString, ShowInInspector, HideLabel]
        private string Name => GetType().Name;
    }
}