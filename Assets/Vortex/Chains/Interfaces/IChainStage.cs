using System;

namespace Vortex.Chains.Model
{
    public interface IChainStage
    {
        /// <summary>
        /// Событие завершения этапа
        /// </summary>
        public event Action OnCompleteStage;

        /// <summary>
        /// Запуск этапа (через проверку условия)
        /// </summary>
        public void Run();

        /// <summary>
        /// Прерывание выполнения этапа
        /// </summary>
        public void Cancel();
    }
}