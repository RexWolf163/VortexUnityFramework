using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Vortex.Unity.MappedParameter.Preset
{
    [Serializable]
    public partial class ParametersMap : ScriptableObject
    {
        /// <summary>
        /// Перечень опорных (дефолтных) параметров
        /// </summary>
        [InfoBox("Обнаружены пустые значения!", InfoMessageType.Error, "Error")]
        [BoxGroup("Базовые параметры")]
        [SerializeField]
        internal string[] baseParams;

        [SerializeReference, OnValueChanged("OnListChanged", true), BoxGroup("Производные параметры")]
        internal MappedParameterPreset[] mappedParams;

#if UNITY_EDITOR

        [Button, BoxGroup("Производные параметры")]
        private void Sort()
        {
            var sortList = new List<string>();
            sortList.AddRange(baseParams);

            foreach (var parent in baseParams)
            {
                var temp = mappedParams
                    .Where(p => p != null && p.Parents.Any(l => l.Parent == parent))
                    .Where(p => !sortList.Contains(p.Name))
                    .OrderBy(p => p.Name)
                    .Select(p => p.Name);
                sortList.AddRange(temp);
            }

            foreach (var mappedParam in mappedParams)
                mappedParam?.Sort();

            mappedParams = mappedParams
                //.OrderBy(x => x.Name)
                .OrderBy(x => sortList.IndexOf(x?.Parents.Length > 0 ? x.Parents[0].Parent : ""))
                .ThenBy(x => x?.Name)
                .ToArray();
        }

        internal List<string> GetParamsNames()
        {
            var result = new List<string>();
            result.AddRange(baseParams);
            foreach (var param in mappedParams)
            {
                if (param == null || param.Name.IsNullOrWhitespace())
                    continue;
                result.Add(param.Name);
            }

            return result;
        }


#endif
    }
}