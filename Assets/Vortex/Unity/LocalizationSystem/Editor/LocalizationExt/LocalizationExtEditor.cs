using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
namespace Vortex.Core.LocalizationSystem.Bus
{
    public partial class Localization
    {
        public static ValueDropdownList<SystemLanguage> GetLanguages()
        {
            var res = new ValueDropdownList<SystemLanguage>();
            var langs = Driver.GetLanguages();
            foreach (var lang in langs)
                res.Add(lang);
            return res;
        }
    }
}
#endif