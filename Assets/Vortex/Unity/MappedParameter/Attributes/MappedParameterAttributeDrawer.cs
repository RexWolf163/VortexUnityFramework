#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Vortex.Unity.Extensions.Editor;

namespace Vortex.Unity.MappedParameter.Attributes
{
    public class MappedParameterAttributeDrawer : OdinAttributeDrawer<MappedParameterAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var type = Attribute.PresetType;
            if (type == null || Activator.CreateInstance(type) is not IMappedParametersGroup model)
            {
                ValueEntry.SmartValue = OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, null);
                return;
            }

            var list = model.GetParametersList();
            ValueEntry.SmartValue = OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
        }
    }
}
#endif