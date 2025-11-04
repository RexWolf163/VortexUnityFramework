using System;

namespace Vortex.Core.UIProviderSystem.Model
{
    /// <summary>
    /// Абстракция условия для интерфейса
    /// После инициализации начинает мониторить заданные параметры, вызывая при их изменении колбэк
    /// по которому должна происходить проверка всех условий UI
    /// </summary>
    public abstract class UserInterfaceCondition
    {
        /// <summary>
        /// Пресет интерфейса-владельца
        /// </summary>
        protected UserInterfaceData Data;

        /// <summary>
        /// Колбэк для активации при изменении состояния проверяемых параметров
        /// </summary>
        protected Action Callback;

        public void Init(UserInterfaceData data, Action callback)
        {
            Data = data;
            Callback = callback;
            Run();
        }

        /// <summary>
        /// Запуск логики
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Останов логики
        /// </summary>
        public abstract void DeInit();

        /// <summary>
        /// Проверка текущего состояния условия
        /// </summary>
        /// <returns></returns>
        public abstract ConditionAnswer Check();
    }
}