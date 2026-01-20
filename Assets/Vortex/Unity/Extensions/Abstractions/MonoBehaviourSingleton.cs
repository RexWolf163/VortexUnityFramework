using UnityEngine;

namespace Vortex.Unity.Extensions.Abstractions
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
        private static T _instance;

        protected static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindAnyObjectByType<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
                Debug.LogError($"[{GetType().Name}: {name}] Singleton already created!");
            _instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}