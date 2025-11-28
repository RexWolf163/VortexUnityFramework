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

        public T GetData<T>() where T : class => _data as T;

        internal void MakeLink(object data)
        {
            _data = data;
            gameObject.SetActive(_data != null);
        }

        internal void Remove() => MakeLink(null);

        private void OnEnable() => CheckState();

        private void CheckState() => gameObject.SetActive(_data != null);
    }
}