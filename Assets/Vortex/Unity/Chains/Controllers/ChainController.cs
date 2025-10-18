using Vortex.Chains.Model;

namespace Vortex.Unity.Chains.Controllers
{
    public static class ChainController
    {
        /// <summary>
        /// Запуск текущего этапа
        /// </summary>
        internal static void RunCurrentStage(this Chain chain)
        {
            if (chain.CurrentStage >= chain.Stages.Length)
                return;
            var stage = chain.Stages[chain.CurrentStage];
            stage.OnCompleteStage += chain.OnStageComplete;
            stage.Run();
        }

        /// <summary>
        /// Прервать выполнение этапа
        /// </summary>
        /// <param name="chain"></param>
        internal static void StopCurrentStage(this Chain chain)
        {
            if (chain.CurrentStage >= chain.Stages.Length)
                return;
            var stage = chain.Stages[chain.CurrentStage];
            stage.OnCompleteStage -= chain.OnStageComplete;
            stage.Cancel();
        }

        /// <summary>
        /// Обработка завершения этапа
        /// </summary>
        private static void OnStageComplete(this Chain chain)
        {
            var stage = chain.Stages[chain.CurrentStage];
            stage.OnCompleteStage -= chain.OnStageComplete;
            chain.CurrentStage++;
            if (chain.CurrentStage >= chain.Stages.Length)
                chain.Complete();
            else
                chain.RunCurrentStage();
        }
    }
}