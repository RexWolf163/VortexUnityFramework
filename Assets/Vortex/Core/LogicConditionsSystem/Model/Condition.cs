using System;

namespace Vortex.Core.LogicConditionsSystem.Model
{
    /// <summary>
    /// Абстракция класса условия 
    /// </summary>
    [Serializable]
    public abstract class Condition
    {
        private Action _onComplete;

        /// <summary>
        /// Инициализация процесса контроля условия
        /// При выполнении условия отработает callback
        /// </summary>
        /// <param name="callback">Метод запуска проверки прочих условий (если их более одного)</param>
        public void Init(Action callback)
        {
            _onComplete = callback;
            Start();
        }

        protected void Complete() => _onComplete?.Invoke();

        protected abstract void Start();

        public abstract void DeInit();

        public abstract bool Check();
    }
}