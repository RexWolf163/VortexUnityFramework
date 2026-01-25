#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Vortex.Core.MappedParametersSystem;
using Vortex.Unity.Extensions.Editor;

namespace Vortex.Unity.MappedParametersSystem.Attributes
{
    public class MappedParameterAttributeDrawer : OdinAttributeDrawer<MappedParameterAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var type = Attribute.PresetType;
            if (ValueEntry.SmartValue == null
                || type == null
                || Activator.CreateInstance(type) is not IMappedParametersGroup model)
            {
                ValueEntry.SmartValue = "";
                ValueEntry.SmartValue =
                    OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, Array.Empty<string>());
                return;
            }

            var list = model.GetParametersList();
            ValueEntry.SmartValue = OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
        }
    }
}
#endif