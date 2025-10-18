using System;
using Vortex.Chains.ChainsInfo;

namespace Vortex.Chains.Model
{
    public interface IChain
    {
        /// <summary>
        /// Событие завершения цепочки
        /// </summary>
        public event Action OnComplete;

        /// <summary>
        /// Иниуиализация
        /// </summary>
        public void Init();

        /// <summary>
        /// Уничтожение и очистка
        /// </summary>
        public void Destroy();

        /// <summary>
        /// Возвращает класс описания для цепочки
        /// </summary>
        /// <returns></returns>
        public ChainInfo GetInfo();
    }
}