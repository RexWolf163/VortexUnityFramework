using System;

namespace Vortex.Chains.Model
{
    [Serializable]
    public abstract class ChainCondition : IChainCondition
    {
        /// <summary>
        /// Событие выполнения условия
        /// </summary>
        public event Action OnConditionComplete;

        /// <summary>
        /// Запуск проверки условия,
        /// при его выполнении должен активироваться event OnConditionComplete
        /// </summary>
        public abstract void Check();

        /// <summary>
        /// прерывание проверок условия
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// Запуск события выполнения условия
        /// </summary>
        protected void Complete() => OnConditionComplete?.Invoke();
    }
}