using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.SettingsSystem.Presets;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace Vortex.Unity.DatabaseSystem.DbSettings
{
    [Serializable]
    public class DatabaseSettings : SettingsPreset
    {
        [InfoBox("Для выбора доступны только те лейблы, которые назначены ассетам!")]
        [SerializeField, ValueDropdown("GetLabels")]
        private string[] databaseLabels;

        public string[] DatabaseLabels => databaseLabels;

#if UNITY_EDITOR
        private string[] GetLabels()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null) return Array.Empty<string>();

            var labels = new HashSet<string>();
            foreach (var group in settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.labels == null)
                        continue;
                    foreach (var label in entry.labels)
                    {
                        if (string.IsNullOrEmpty(label))
                            continue;
                        labels.Add(label);
                    }
                }
            }

            return labels.OrderBy(l => l).ToArray();
        }
#endif
    }
}