using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.LocalizationSystem.Bus
{
    public class Localization : SystemController<Localization, IDriver>
    {
        /// <summary>
        /// Значение текущей локали
        /// </summary>
        private static SystemLanguage _currentLanguage;

        /// <summary>
        /// Значение текущей локали
        /// </summary>
        private static SystemLanguage СurrentLanguage => _currentLanguage;

        /// <summary>
        /// Индекс локализованных фрагментов
        /// </summary>
        private static SortedDictionary<string, string> index = new();

        protected override void OnDriverConnect()
        {
            _currentLanguage = Driver.GetDefaultLanguage();
            Driver.SetIndex(index);
        }

        protected override void OnDriverDisonnect()
        {
        }

        /// <summary>
        /// Узнать текущую локаль приложения 
        /// </summary>
        /// <returns></returns>
        public static SystemLanguage GetCurrentLanguage() => СurrentLanguage;

        /// <summary>
        /// Установить язык для приложения
        /// </summary>
        /// <param name="language"></param>
        public static void SetCurrentLanguage(SystemLanguage language) => Driver.SetLanguage(language);

        /// <summary>
        /// Установить язык для приложения
        /// </summary>
        /// <param name="language"></param>
        public static void SetCurrentLanguage(string language)
        {
            if (!Enum.TryParse(typeof(SystemLanguage), language, true, out var result))
                return;
            if (result != null)
                SetCurrentLanguage((SystemLanguage)result);
        }

        /// <summary>
        /// Возвращает ассоциацию с ключом в текущей локали приложения
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetTranslate(string key) => index[key];
    }
}