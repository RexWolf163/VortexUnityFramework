using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.BehaviorLogics
{
    public class OverlayBehavior : UserInterfaceBehavior
    {
        public override void Init(UserInterface userInterface) => UI = userInterface;

        public override void DeInit()
        {
        }
    }
}