using System;
using Sirenix.OdinInspector;

namespace Vortex.Unity.UIProviderSystem.Model.Conditions
{
    [Serializable]
    public abstract class UserInterfaceCondition
    {
        [DisplayAsString, ShowInInspector, HideLabel]
        private string Name => GetType().Name;

        protected UserInterfaceData Data;
        protected Action Callback;

        public void Init(UserInterfaceData data, Action callback)
        {
            Data = data;
            Callback = callback;
            Run();
        }

        protected abstract void Run();
        public abstract void DeInit();

        public abstract ConditionAnswer Check();
    }
}