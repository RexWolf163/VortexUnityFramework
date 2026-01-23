using System.Collections.Generic;
using System.Linq;
using Vortex.Unity.MappedParameter.Preset;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.MappedParameter
{
    /// <summary>
    /// Базовый класс для формирования группы связанных параметров
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MappedParametersGroup<T> : IMappedParametersGroup where T : ParametersMap
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

        public void Init()
        {
            if (_map == null)
            {
                LoadMap();
            }
            else
            {
                if (mappedParameter == null)
                {
                    Debug.LogError($"[{GetType().Name}] Критическая ошибка инициализации!");
                    return;
                }

                foreach (var parameter in mappedParameter)
                    parameter.SetValue(0);
            }
        }

        private void LoadMap()
        {
            var resAr = Resources.LoadAll<T>("");
            if (resAr.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}] Не обнаружено пресетов ({typeof(T)}) для данного класса");
                return;
            }

            if (resAr.Length > 1)
                Debug.LogError(
                    $"[{GetType().Name}] Обнаружено несколько пресетов ({typeof(T)}) для данного класса");

            _map = resAr[0];
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
        private GenericParameter[] GetParents(string paramName)
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
        private MappedParameterLink[] GetCost(string paramName)
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

#if UNITY_EDITOR

        /// <summary>
        /// Список параметров для выпадашки
        /// </summary>
        /// <returns></returns>
        public string[] GetParametersList()
        {
            LoadMap();
            return _parametersIndex.Keys.ToArray();
        }
#endif
    }
}