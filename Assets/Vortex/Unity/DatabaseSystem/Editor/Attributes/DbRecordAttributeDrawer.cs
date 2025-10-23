using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Vortex.Unity.Extensions.Editor;

namespace Vortex.Unity.DatabaseSystem.Editor.Attributes
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