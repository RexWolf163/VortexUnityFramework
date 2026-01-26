using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions.Actions;
using Vortex.Core.MappedParametersSystem.Base;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.MappedParametersSystem.Handlers
{
    /// <summary>
    /// Класс-источник данных
    /// Настраивается на MappedModelStorage 
    /// </summary>
    public class MappedParameterStorage : MonoBehaviour, IDataStorage
    {
        /// <summary>
        /// Событие изменение модели
        /// </summary>
        public event Action OnUpdate;

        private GenericParameter _data;

        private GenericParameter Data
        {
            get
            {
                if (_data == null)
                {
                    _data = _storage.GetData<IMappedModel>()?.GetParameterAsContainer(parameterName);
                    if (_data == null)
                        return null;
                    _data.OnUpdate += CallOnUpdate;
                }

                return _data;
            }
        }

        private void CallOnUpdate() => OnUpdate.Fire();


        [ShowInInspector] private MappedModelStorage _storage;

        [SerializeField, ValueDropdown("GetParametersList")]
        private string parameterName;

        public T GetData<T>() where T : class => Data.Value as T;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_storage == null)
                _storage = GetComponent<MappedModelStorage>();
            if (_storage == null)
                _storage = GetComponentInParent<MappedModelStorage>();
        }

        private string[] GetParametersList() =>
            _storage.GetData<IMappedModel>()?.GetParameters() ?? Array.Empty<string>();
#endif
    }
}