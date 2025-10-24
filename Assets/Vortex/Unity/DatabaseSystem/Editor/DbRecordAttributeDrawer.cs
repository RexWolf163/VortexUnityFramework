#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Editor.Attributes;
using Vortex.Unity.Extensions.Editor;

namespace Vortex.Unity.DatabaseSystem.Editor
{
    public class DbRecordAttributeDrawer : OdinAttributeDrawer<DbRecordAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var list = DatabaseDriver.Instance.GetDropdownList();
            ValueEntry.SmartValue = OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
        }
    }
}
#endif