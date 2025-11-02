using Vortex.Unity.UIProviderSystem.Bus;

namespace Vortex.Unity.UIProviderSystem.Model.Conditions
{
    public class CloseOnOpenAnyUICondition : UserInterfaceCondition
    {
        protected override void Run()
        {
            UIProvider.OnOpen += Callback;
        }

        public override void DeInit()
        {
            UIProvider.OnOpen -= Callback;
        }

        public override ConditionAnswer Check()
        {
            var list = UIProvider.GetOpenedUIs();
            if (list.Contains(Data) && list.Count == 1)
                return ConditionAnswer.Idle;
            return ConditionAnswer.Close;
        }
    }
}