using System;
using System.Collections.Generic;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.LocalizationSystem.Bus
{
    public partial class Localization : SystemController<Localization, IDriver>
    {
        /// <summary>
        /// Событие смены языка локали
        /// </summary>
        public static event Action OnLocalizationChanged;

        /// <summary>
        /// Значение текущей локали
        /// </summary>
        private static string _currentLanguage;

        /// <summary>
        /// Значение текущей локали
        /// </summary>
        private static string СurrentLanguage => _currentLanguage;

        /// <summary>
        /// Индекс локализованных фрагментов
        /// </summary>
        private static SortedDictionary<string, string> index = new();

        protected override void OnDriverConnect()
        {
            _currentLanguage = Driver.GetDefaultLanguage();
            Driver.SetIndex(index);
            Driver.OnLocalizationChanged += CallOnLocalization;
        }

        protected override void OnDriverDisonnect()
        {
            Driver.OnLocalizationChanged -= CallOnLocalization;
        }

        /// <summary>
        /// Узнать текущую локаль приложения 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguage() => СurrentLanguage;

        /// <summary>
        /// Установить язык для приложения
        /// </summary>
        /// <param name="language"></param>
        public static void SetCurrentLanguage(string language)
        {
            _currentLanguage = language;
            Driver.SetLanguage(language);
        }

        /// <summary>
        /// Возвращает ассоциацию с ключом в текущей локали приложения
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetTranslate(string key) => index.ContainsKey(key) ? index[key] : $"##!{key}!##";

        /// <summary>
        /// Проверка есть ли такой ключ в реестре
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasTranslate(string key) => index.ContainsKey(key);

        private static void CallOnLocalization() => OnLocalizationChanged?.Invoke();
    }
}