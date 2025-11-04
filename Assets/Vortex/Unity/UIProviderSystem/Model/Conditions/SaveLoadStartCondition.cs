using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.SaveSystem.Model;
using Vortex.Core.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Model.Conditions
{
    /// <summary>
    /// Условие: Показывать пока идет процесс загрузки или сохранения
    /// (источник событий SaveController)
    /// </summary>
    public class SaveLoadStartCondition : UnityUserInterfaceCondition
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