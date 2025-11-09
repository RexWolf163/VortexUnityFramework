using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Object = UnityEngine.Object;

namespace Vortex.Unity.UI.PoolSystem
{
    [Serializable]
    public class Pool : MonoBehaviour
    {
        [SerializeField] private PoolItem preset;

        private readonly Dictionary<PoolItem, Object> _index = new();

        private void Awake()
        {
            _index.Clear();
            var list = GetComponentsInChildren<PoolItem>();
            foreach (var item in list)
            {
                _index.Add(item, null);
                item.MakeLink(null);
            }
        }

        private void OnDestroy()
        {
        }

        private void OnEnable()
        {
            CheckState();
        }

        public void AddItem(Object data)
        {
            var item = CreateItem();
            _index.AddNew(item, data);
            item.MakeLink(data);
        }

        private PoolItem CreateItem()
        {
            var keys = _index.Keys;
            PoolItem item = null;
            foreach (var poolItem in keys)
            {
                if (_index[poolItem] == null)
                {
                    item = poolItem;
                    break;
                }
            }

            if (item == null)
                item = Instantiate(preset);

            return item;
        }

        private void CheckState()
        {
            foreach (var item in _index)
                item.Key.MakeLink(item.Value);
        }
    }
}