using System;

namespace Vortex.Chains
{
    public interface IChainCondition
    {
        public event Action OnConditionComplete;

        /// <summary>
        /// Запуск проверки условия,
        /// при его выполнении должен активироваться event OnConditionComplete
        /// </summary>
        public void Check();

        /// <summary>
        /// прерывание проверок условия
        /// </summary>
        public void Cancel();
    }
}