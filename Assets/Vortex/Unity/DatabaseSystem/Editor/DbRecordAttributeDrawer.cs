#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.Extensions.Editor;

namespace Vortex.Unity.DatabaseSystem.Editor
{
    public class DbRecordAttributeDrawer : OdinAttributeDrawer<DbRecordAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var list = DatabaseDriver.Instance.GetDropdownList(Attribute.RecordClass, Attribute.RecordType);

            EditorGUI.BeginChangeCheck();
            var color = GUI.color;
            var test = TestMethod();
            if (!test)
                GUI.color = Color.red;
            ValueEntry.SmartValue =
                OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
            if (EditorGUI.EndChangeCheck())
                TestMethod();
            if (!test)
                GUI.color = color;
        }

        private bool TestMethod() =>
            !ValueEntry.SmartValue.IsNullOrWhitespace() && Database.TestRecord(ValueEntry.SmartValue);
    }
}
#endif