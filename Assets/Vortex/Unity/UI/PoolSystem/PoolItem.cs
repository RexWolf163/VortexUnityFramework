using System;
using UnityEngine;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.UI.PoolSystem
{
    /// <summary>
    /// Элемент-Контейнер для пула
    /// Реализует интерфейс IDataStorage. Хранит инициализирующие данные
    /// </summary>
    [Serializable]
    public class PoolItem : MonoBehaviour, IDataStorage
    {
        private object _data;

        private Pool _owner;

        public T GetData<T>() where T : class => _data as T;

        internal void MakeLink(object data, Pool pool)
        {
            _data = data;
            _owner = pool;
            gameObject.SetActive(_data != null);
        }

        internal void Remove() => MakeLink(null, _owner);

        private void OnEnable() => CheckState();

        private void OnDisable()
        {
            if (_data != null && _owner != null)
                SelfDestroy();
        }

        private void CheckState() => gameObject.SetActive(_data != null);

        private void SelfDestroy() => _owner.RemoveItem(_data);
    }
}