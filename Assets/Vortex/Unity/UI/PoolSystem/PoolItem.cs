using System;
using UnityEngine;
using Vortex.Core.System.Abstractions;
using Object = UnityEngine.Object;

namespace Vortex.Unity.UI.PoolSystem
{
    [Serializable]
    public class PoolItem : MonoBehaviour, IDataStorage
    {
        private Object _data;

        public T GetData<T>() where T : class => _data as T;

        public void MakeLink(Object data)
        {
            _data = data;
            gameObject.SetActive(_data != null);
        }

        private void OnEnable() => CheckState();

        private void CheckState() => gameObject.SetActive(_data != null);
    }
}