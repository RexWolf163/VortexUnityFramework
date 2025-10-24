#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Vortex.Unity.Extensions.Editor;
using Vortex.Unity.UIProviderSystem.Attributes;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Editor
{
    public class UserInterfaceAttributeDrawer : OdinAttributeDrawer<UserInterfaceAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var list = new ValueDropdownList<string>();
            var currentDomain = AppDomain.CurrentDomain;
            var assems = currentDomain.GetAssemblies();
            foreach (var assembly in assems)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                    if (type.IsSubclassOf(typeof(UserInterface)))
                        list.Add(new ValueDropdownItem<string>(type.Name, type.AssemblyQualifiedName));
            }

            ValueEntry.SmartValue = OdinDropdownTool.DropdownSelector(label, ValueEntry.SmartValue, list);
        }
    }
}
#endif