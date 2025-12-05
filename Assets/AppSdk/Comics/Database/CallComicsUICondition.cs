using Sirenix.OdinInspector;
using Vortex.Core.UIProviderSystem.Model;

namespace AppSdk.Comics.Database
{
    public class CallComicsUICondition : UserInterfaceCondition
    {
        [DisplayAsString, ShowInInspector, HideLabel]
        private string Name => GetType().Name;

        private bool opened;

        protected override void Run()
        {
            ComicsController.OnCallComics += RunCallback;
            ComicsController.OnStopComics += RunCallback;
        }

        public override void DeInit()
        {
            ComicsController.OnCallComics -= RunCallback;
            ComicsController.OnStopComics -= RunCallback;
        }

        public override ConditionAnswer Check()
        {
            if (ComicsController.GetCurrentComics() != null)
            {
                opened = true;
                return ConditionAnswer.Open;
            }

            if (opened)
            {
                opened = false;
                return ConditionAnswer.Close;
            }

            return ConditionAnswer.Idle;
        }
    }
}