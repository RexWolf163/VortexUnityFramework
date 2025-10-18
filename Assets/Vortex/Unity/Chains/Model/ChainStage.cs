using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.App;
using Vortex.Core.Enums;

namespace Vortex.Chains.Model
{
    /// <summary>
    /// Абстрактная основа для этапов цепочки
    /// </summary>
    [Serializable]
    public abstract class ChainStage : IChainStage
    {
        [SerializeReference, HideReferenceObjectPicker]
        private ChainCondition _condition;

        /// <summary>
        /// Событие завершения этапа
        /// </summary>
        public event Action OnCompleteStage;

        /// <summary>
        /// Запуск этапа (через проверку условия)
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            Debug.Log($"Run {GetType().Name}");
            AppController.Data.OnStateChanged += CheckAppState;
            if (_condition == null)
            {
                Logic();
                return;
            }

            _condition.OnConditionComplete += RunLogic;
            _condition.Check();
        }

        private void CheckAppState(AppStates state)
        {
            if (state == AppStates.Stopping)
                Cancel();
        }

        /// <summary>
        /// Запуск логики этапа при срабатывании события
        /// </summary>
        private void RunLogic()
        {
            _condition.OnConditionComplete -= RunLogic;
            Logic();
        }

        /// <summary>
        /// Завершение этапа
        /// </summary>
        protected void Complete() => OnCompleteStage?.Invoke();

        /// <summary>
        /// Логика этапа.
        /// По выполнении должна вызвать событие Complete()
        /// </summary>
        protected abstract void Logic();

        /// <summary>
        /// Прервать выполнение этапа
        /// </summary>
        public void Cancel()
        {
            if (_condition != null)
                _condition.OnConditionComplete -= RunLogic;
            Stop();
        }

        protected abstract void Stop();
    }
}