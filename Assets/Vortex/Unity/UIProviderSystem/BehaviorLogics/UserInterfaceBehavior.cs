using System;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.BehaviorLogics
{
    [Serializable]
    public abstract class UserInterfaceBehavior
    {
        protected UserInterface UI;

        public abstract void Init(UserInterface userInterface);
        public abstract void DeInit();
    }
}