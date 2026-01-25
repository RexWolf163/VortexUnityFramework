using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.MappedParametersSystem;
using Vortex.Core.MappedParametersSystem.Base;
using Vortex.Unity.MappedParametersSystem.Base.Preset;

namespace Vortex.Unity.MappedParametersSystem.Base
{
    /// <summary>
    /// Класс контейнер для группы связанных параметров
    /// </summary>
    public class MappedParametersGroup : IMappedParametersGroup
    {
        /// <summary>
        /// Список параметров
        /// </summary>
        [SerializeReference, HideReferenceObjectPicker]
        private GenericParameter[] mappedParameter;

        private ParametersMap _map;

        /// <summary>
        /// Индекс для быстрого доступа по названию
        /// </summary>
        private Dictionary<string, GenericParameter> _parametersIndex = new();

        public MappedParametersGroup(ParametersMap map) => Init(map);

        /// <summary>
        /// Возвращает родителя параметра
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public GenericParameter[] GetParents(GenericParameter param) => GetParents(param.Name);

        /// <summary>
        /// Возвращает родителя параметра
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public GenericParameter[] GetParents(string paramName)
        {
            if (_map == null || _map.baseParams.Contains(paramName))
                return null;
            var paramData = _map.mappedParams.FirstOrDefault(p => p.Name == paramName);
            if (paramData == null)
                return new GenericParameter[0];
            var parentsNames = paramData.Parents
                .Select(p => p.Parent)
                .ToArray();
            var parents = _parametersIndex
                .Where(p => parentsNames.Contains(p.Key))
                .Select(p => p.Value).ToArray();
            return parents;
        }

        /// <summary>
        /// Возвращает значение стоимости связи в виде массива с параметром названия родителя и связанной с ним цифры
        /// Смысл этой цифры определяется логикой контроллера
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MappedParameterLink[] GetCost(GenericParameter param) => GetCost(param.Name);

        /// <summary>
        /// Возвращает значение стоимости связи в виде массива с параметром названия родителя и связанной с ним цифры
        /// Смысл этой цифры определяется логикой контроллера
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public MappedParameterLink[] GetCost(string paramName)
        {
            if (_map == null || _map.baseParams.Contains(paramName))
                return null;
            var paramData = _map.mappedParams.FirstOrDefault(p => p.Name == paramName);
            if (paramData == null)
                return new MappedParameterLink[0];
            var parentsNames = paramData.Parents.ToArray();
            return parentsNames;
        }

        /// <summary>
        /// Возвращает значение указанного параметра
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public int GetValue(string paramName) =>
            _parametersIndex.TryGetValue(paramName, out var parent) ? parent.Value : 0;

        /// <summary>
        /// Список параметров для выпадашки
        /// </summary>
        /// <returns></returns>
        public string[] GetParametersList()
        {
            //TODO пофиксить
            return _parametersIndex.Keys.ToArray();
        }

        private void Init(ParametersMap mapAsset)
        {
            _map = mapAsset;
            mappedParameter = new GenericParameter[_map.baseParams.Length + _map.mappedParams.Length];

            var c = _map.baseParams.Length;
            for (var i = 0; i < c; i++)
            {
                var param = _map.baseParams[i];
                mappedParameter[i] = new GenericParameter(param);
                _parametersIndex.Add(mappedParameter[i].Name, mappedParameter[i]);
            }

            for (var i = _map.mappedParams.Length - 1 + c; i >= c; i--)
            {
                var param = _map.mappedParams[i - c];
                mappedParameter[i] = new GenericParameter(param.Name);
                _parametersIndex.Add(mappedParameter[i].Name, mappedParameter[i]);
            }
        }
    }
}