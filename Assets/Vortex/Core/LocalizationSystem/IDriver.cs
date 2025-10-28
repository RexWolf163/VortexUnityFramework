using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.LocalizationSystem
{
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Получить дефолтный язык для приложения (при инициации)
        /// </summary>
        /// <returns></returns>
        public SystemLanguage GetDefaultLanguage();

        /// <summary>
        /// Установить язык для приложения
        /// </summary>
        /// <param name="language"></param>
        public void SetLanguage(SystemLanguage language);

        /// <summary>
        /// Связь индекса с данными драйвера
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(SortedDictionary<string, string> index);

        /// <summary>
        /// Событие смены языка локали
        /// </summary>
        public event Action OnLocalizationChanged;

        
        public SystemLanguage[] GetLanguages();
    }
}