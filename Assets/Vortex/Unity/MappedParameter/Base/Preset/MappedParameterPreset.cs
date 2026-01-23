using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Unity.Extensions.Attributes;

namespace Vortex.Unity.MappedParameter.Preset
{
    [HideReferenceObjectPicker, Serializable, FoldoutClass("$GetFoldoutName")]
    public class MappedParameterPreset
    {
        [SerializeField] internal string name;

        /// <summary>
        /// Название параметра
        /// </summary>
        public string Name => name;

        [SerializeReference, OnValueChanged("OnListChanged")]
        internal MappedParameterLink[] parents = new MappedParameterLink[0];

        /// <summary>
        /// Родители для параметра
        /// </summary>
        public MappedParameterLink[] Parents => parents;

        /// <summary>
        /// Логика проверки стоимости (справочный параметр)
        /// </summary>
        [SerializeField, HideIf("@parents.Length <= 1")]
        internal MappedParameterParentCostLogic costLogic;

        public MappedParameterParentCostLogic CostLogic => costLogic;

#if UNITY_EDITOR
        /// <summary>
        /// Карта-владелец параметра
        /// В редакторе должна передаваться внутрь этого класса через EditorInit
        /// </summary>
        private ParametersMap map;

        internal void EditorInit(ParametersMap map)
        {
            this.map = map;
            foreach (var parent in parents)
                parent.EditorInit(map, this);
        }

        /// <summary>
        /// Сортировка списка родителей
        /// </summary>
        internal void Sort() => parents = parents.OrderBy(x => x.Parent).ToArray();

        private string GetFoldoutName()
        {
            if (Parents.IsNullOrEmpty())
                return $"{(name.IsNullOrWhitespace() ? "???" : name)} ( ??? )";
            if (Parents.Length == 1)
                return $"{(name.IsNullOrWhitespace() ? "???" : name)} ({parents[0].Parent} x {parents[0].Cost}pts.)";
            return $"{(name.IsNullOrWhitespace() ? "???" : name)} ({parents.Length} parents)";
        }

        private List<string> GetParentVariants() => map.GetParamsNames();

        private void OnListChanged() => EditorInit(map);
#endif
    }
}