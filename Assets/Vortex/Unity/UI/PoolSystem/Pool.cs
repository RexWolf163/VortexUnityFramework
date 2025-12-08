using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;

namespace Vortex.Unity.UI.PoolSystem
{
    [Serializable]
    public class Pool : MonoBehaviour
    {
        /// <summary>
        /// Образец контейнера
        /// </summary>
        [SerializeField] private PoolItem preset;

        /// <summary>
        /// Реестр активных контейнеров
        /// </summary>
        private readonly Dictionary<object, PoolItem> _index = new();

        /// <summary>
        /// Перечень-очередь обнуленных контейнеров
        /// </summary>
        private readonly Queue<PoolItem> _freeItems = new();

        private void Awake()
        {
            _index.Clear();
            /*
             //TODO решить: нужно ли трогать образец?
            var list = GetComponentsInChildren<PoolItem>();
            foreach (var item in list)
                _freeItems.Enqueue(item);
            */
        }

        private void OnDestroy() => Clear();

        /// <summary>
        /// Добавить элемент пула для данных
        /// </summary>
        /// <param name="data"></param>
        public void AddItem(object data)
        {
            if (_index.ContainsKey(data))
                return;
            var item = CreateItem();
            item.transform.SetAsLastSibling();
            _index.AddNew(data, item);
            item.MakeLink(data);
        }

        /// <summary>
        /// Добавить элемент пула для данных.
        /// Вернуть компонент указанного типа из контейнера
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddItem<T>(object data) where T : MonoBehaviour
        {
            if (_index.ContainsKey(data))
                return GetItem<T>(data);
            AddItem(data);
            return GetItem<T>(data);
        }

        /// <summary>
        /// Вернуть компонент указанного типа из контейнера связанного с данными
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetItem<T>(object data) where T : MonoBehaviour =>
            _index.TryGetValue(data, out var item) ? item.GetComponentInChildren<T>() : null;

        /// <summary>
        /// Создать новый контейнер в пуле
        /// </summary>
        /// <returns></returns>
        private PoolItem CreateItem() => _freeItems.Count > 0
            ? _freeItems.Dequeue()
            : Instantiate(preset, Vector3.zero, new Quaternion(0, 0, 0, 0), transform);

        /// <summary>
        /// Удалить контейнер
        /// </summary>
        /// <param name="data"></param>
        public void RemoveItem(object data)
        {
            if (!_index.TryGetValue(data, out var value))
                return;
            value.Remove();
            _freeItems.Enqueue(value);
            _index.Remove(data);
        }

        /// <summary>
        /// очистка контейнера
        /// </summary>
        public void Clear()
        {
            var list = _index.Keys.ToArray();
            foreach (var data in list)
                RemoveItem(data);
        }
    }
}