#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Vortex.Unity.Extensions.Editor;
using UnityEngine;
using Vortex.Core.LocalizationSystem.Bus;

namespace Vortex.Unity.LocalizationSystem.Editor
{
    public class LanguageAttributeDrawer : OdinAttributeDrawer<LanguageAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var list = Localization.GetLanguages();
            ValueEntry.SmartValue =
                OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
        }
    }
}
#endif