#if UNITY_EDITOR
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Vortex.Core.LocalizationSystem.Bus
{
    public partial class Localization
    {
        [MenuItem("Vortex/Localization/Set Default Locale")]
        public static void SetDefaultLocale()
        {
            SetCurrentLanguage(Application.systemLanguage.ToString());
        }

        [MenuItem("Vortex/Localization/Set Next Locale")]
        public static void SetNextLocale()
        {
            var langs = Driver.GetLanguages();
            var currentLang = GetCurrentLanguage();
            var index = Array.IndexOf(langs, currentLang) + 1;
            if (index < 0 || index >= langs.Length)
                index = 0;

            SetCurrentLanguage(langs[index]);
        }

        public static ValueDropdownList<string> GetLanguages()
        {
            var res = new ValueDropdownList<string>();
            var langs = Driver.GetLanguages();
            foreach (var lang in langs)
                res.Add(lang);
            return res;
        }

        public static ValueDropdownList<string> GetLocalizationKeys()
        {
            var res = new ValueDropdownList<string>();
            var texts = index.Keys.ToArray();
            foreach (var value in texts)
                res.Add(value);
            return res;
        }
    }
}
#endif