using System;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace Vortex.Unity.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DateTimeDrawAttribute : Attribute
    {
    }

#if UNITY_EDITOR
    public sealed class DateTimeAttributeDrawer : OdinAttributeDrawer<DateTimeDrawAttribute, long>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = EditorGUILayout.GetControlRect();

            GUIHelper.PushLabelWidth(100);
            ValueEntry.SmartValue = Math.Max(0,
                SirenixEditorFields.LongField(rect.AlignLeft(rect.width * 0.6f), label, ValueEntry.SmartValue));
            GUI.enabled = false;
            SirenixEditorFields.TextField(rect.AlignRight(rect.width * 0.4f), ConvertToTime(ValueEntry.SmartValue));
            GUI.enabled = true;
            GUIHelper.PopLabelWidth();
        }

        private static string ConvertToTime(long timeSpan)
        {
            var time = TimeSpan.FromSeconds(timeSpan);
            return time.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
#endif
}