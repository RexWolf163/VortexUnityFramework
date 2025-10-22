using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.BehaviorLogics
{
    public class PanelBehavior : UserInterfaceBehavior
    {
        public override void Init(UserInterface userInterface) => UI = userInterface;

        public override bool CheckOpenRule() =>
            UI.State != UserInterfaceStates.Show && UI.State != UserInterfaceStates.Showing;

        public override void DeInit()
        {
        }
    }
}