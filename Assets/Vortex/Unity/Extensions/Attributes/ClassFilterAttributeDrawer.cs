using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Vortex.Unity.Extensions.Attributes
{
    [CustomPropertyDrawer(typeof(ClassFilterAttribute))]
    public class ClassFilterAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var filterAttr = (ClassFilterAttribute)attribute;

            EditorGUI.BeginProperty(position, label, property);
            var obj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);

            if (obj != property.objectReferenceValue)
            {
                bool isValid = IsValidObject(obj, filterAttr.RequiredType);
                if (isValid)
                {
                    property.objectReferenceValue = obj;
                }
                else
                {
                    property.objectReferenceValue = null;
                    if (obj != null)
                    {
                        Debug.LogWarning(
                            $"Объект '{obj.name}' не соответствует типу {filterAttr.RequiredType.Name}. Поле очищено.");
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        private bool IsValidObject(Object obj, Type requiredType)
        {
            if (obj == null) return true;
            return obj switch
            {
                MonoBehaviour mb => requiredType.IsAssignableFrom(mb.GetType()),
                ScriptableObject so => requiredType.IsAssignableFrom(so.GetType()),
                _ => false
            };
        }
    }
}