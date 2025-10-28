using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Unity.Extensions.Editor;

namespace Vortex.Unity.LocalizationSystem.Editor
{
    public class LanguageAttributeDrawer : OdinAttributeDrawer<LanguageAttribute, SystemLanguage>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var list = Localization.GetLanguages();
            ValueEntry.SmartValue =
                OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
        }
    }
}