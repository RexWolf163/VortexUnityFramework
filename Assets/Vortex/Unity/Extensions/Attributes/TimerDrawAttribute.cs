using System;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif

namespace Vortex.Unity.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TimerDrawAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DateTimerDrawAttribute : Attribute
    {
    }

#if UNITY_EDITOR
    public sealed class TimerAttributeDrawer : OdinAttributeDrawer<TimerDrawAttribute, long>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUI.enabled = false;
            SirenixEditorFields.TextField(label, ConvertToTime(ValueEntry.SmartValue));
            GUI.enabled = true;
        }

        private string ConvertToTime(long timeSpan)
        {
            var time = TimeSpan.FromSeconds(timeSpan);
            if (time.Days > 0)
                return $"{time:d\\d\\ hh\\:mm\\:ss}";
            return $"{time:hh\\:mm\\:ss}";
        }
    }

    public sealed class DateTimerAttributeDrawer : OdinAttributeDrawer<DateTimerDrawAttribute, long>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUI.enabled = false;
            SirenixEditorFields.TextField(label, ConvertToTime(ValueEntry.SmartValue));
            GUI.enabled = true;
        }

        private string ConvertToTime(long timeSpan)
        {
            var time = TimeSpan.FromSeconds(timeSpan);
            return time.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
#endif
}