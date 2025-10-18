using System;

namespace Vortex.Chains.ChainsInfo
{
    /// <summary>
    /// Класс описания состояния логической цепочки.
    /// Обновляется из "Условий"
    /// </summary>
    public class ChainInfo
    {
        /// <summary>
        /// Паттерн описания
        /// </summary>
        internal string textPattern;

        /// <summary>
        /// Параметры для паттерна описания
        /// </summary>
        internal Object[] textParams = Array.Empty<Object>();

        /// <summary>
        /// Дополнительные параметры описания
        /// (Например список предметов, которые ожидаются)
        /// </summary>
        internal Object[] data = Array.Empty<Object>();

        /// <summary>
        /// Возвращает текст описания сформированный из паттерна и данных
        /// </summary>
        /// <returns></returns>
        public string GetText() => String.Format(textPattern, textParams);

        /// <summary>
        /// Возвращает дополнительные данные описания (Например список предметов, которые ожидаются) 
        /// </summary>
        /// <returns></returns>
        public Object[] GetData() => data;
    }
}