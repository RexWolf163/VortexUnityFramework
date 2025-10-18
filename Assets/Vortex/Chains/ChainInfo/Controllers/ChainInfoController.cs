using System;

namespace Vortex.Chains.ChainsInfo
{
    /// <summary>
    /// Контроллер управления моделью ChainInfo
    /// </summary>
    public static class ChainInfoController
    {
        /// <summary>
        /// Выставляет паттерн для вывода описания
        /// </summary>
        /// <param name="chainInfo"></param>
        /// <param name="pattern"></param>
        public static void SetChainInfoPattern(this ChainInfo chainInfo, string pattern) =>
            chainInfo.textPattern = pattern;

        /// <summary>
        /// Выставляет данные для вывода текст описания по установленному паттерну
        /// </summary>
        /// <param name="chainInfo"></param>
        /// <param name="patternParams"></param>
        public static void SetChainInfoParams(this ChainInfo chainInfo, Object[] patternParams) =>
            chainInfo.textParams = patternParams;

        /// <summary>
        /// Выставляет дополнительные данные целей текущего условия цепочки (например список затребованных предметов) 
        /// </summary>
        /// <param name="chainInfo"></param>
        /// <param name="data"></param>
        public static void SetChainInfoPattern(this ChainInfo chainInfo, Object[] data) =>
            chainInfo.data = data;
    }
}