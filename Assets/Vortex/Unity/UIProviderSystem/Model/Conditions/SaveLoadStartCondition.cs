using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.SaveSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Model.Conditions
{
    public class SaveLoadStartCondition : UserInterfaceCondition
    {
        protected override void Run()
        {
            SaveController.OnLoadStart += Call;
            SaveController.OnLoadComplete += Call;
            SaveController.OnSaveStart += Call;
            SaveController.OnSaveComplete += Call;
        }

        public override void DeInit()
        {
            SaveController.OnLoadStart -= Call;
            SaveController.OnLoadComplete -= Call;
            SaveController.OnSaveStart -= Call;
            SaveController.OnSaveComplete -= Call;
        }

        public override ConditionAnswer Check() => SaveController.State != SaveControllerStates.Idle
            ? ConditionAnswer.Open
            : ConditionAnswer.Close;

        private void Call() => Callback?.Invoke();
    }
}