using System;
using Vortex.Core.LocalizationSystem.Bus;

namespace Vortex.Core.LocalizationSystem
{
    public static class StringExt
    {
        /// <summary>
        /// Возвращает ассоциацию с ключом в текущей локали приложения
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Translate(this String key) => Localization.GetTranslate(key);
    }
}