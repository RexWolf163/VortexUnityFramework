using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.LocalizationSystem;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.LocalizationSystem
{
    public partial class LocalizationDriver : Singleton<LocalizationDriver>, IDriver
    {
        private const string Path = "Localization";
        private const string SaveSlot = "AppLanguage";

        private static SortedDictionary<string, string> _localeData;

        public event Action OnInit;

        public void Init()
        {
        }

        public void Destroy()
        {
        }

        /// <summary>
        /// Связь индекса с данными драйвера
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(SortedDictionary<string, string> index) => _localeData = index;

        /// <summary>
        /// Получить дефолтный язык для приложения (при инициации)
        /// </summary>
        /// <returns></returns>
        public SystemLanguage GetDefaultLanguage()
        {
            if (PlayerPrefs.HasKey(SaveSlot))
            {
                Enum.TryParse(typeof(SystemLanguage), PlayerPrefs.GetString(SaveSlot), true, out var result);
                return result == null ? Application.systemLanguage : (SystemLanguage)result;
            }

            return Application.systemLanguage;
        }

        /// <summary>
        /// Установить язык для приложения
        /// </summary>
        /// <param name="language"></param>
        public void SetLanguage(SystemLanguage language)
        {
            PlayerPrefs.SetString(SaveSlot, language.ToString());
            Loader.RunAlone(this);
        }
    }
}