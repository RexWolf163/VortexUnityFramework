using AppScripts.Narrative;
using Vortex.Core.UIProviderSystem.Model;
using Vortex.Unity.UIProviderSystem.Model;

namespace AppScripts.NarrativeNavigator.View
{
    public class NarrativeDialogueUICondition : UnityUserInterfaceCondition
    {
        protected override void Run()
        {
            NarrativeController.OnDialogueStateChanged += RunCallback;
            RunCallback();
        }

        public override void DeInit()
        {
            NarrativeController.OnDialogueStateChanged -= RunCallback;
        }

        public override ConditionAnswer Check() =>
            NarrativeController.HasDialogue ? ConditionAnswer.Open : ConditionAnswer.Close;
    }
}