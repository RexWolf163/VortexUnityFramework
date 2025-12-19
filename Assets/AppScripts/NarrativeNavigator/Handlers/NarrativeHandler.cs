using AppScripts.Narrative;
using UnityEngine;
using UnityEngine.Events;

namespace AppScripts.NarrativeNavigator.Handlers
{
    /// <summary>
    /// Хэндлер управления загрузкой интерфейсов при запуске или завершении сюжета
    /// </summary>
    public class NarrativeHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent loadSceneCallback;
        [SerializeField] private UnityEvent unLoadSceneCallback;

        private void OnEnable()
        {
            NarrativeController.OnStart += LoadScene;
            NarrativeController.OnComplete += UnLoadScene;
        }

        private void OnDisable()
        {
            NarrativeController.OnStart -= LoadScene;
            NarrativeController.OnComplete -= UnLoadScene;
        }

        private void LoadScene() => loadSceneCallback?.Invoke();
        private void UnLoadScene() => unLoadSceneCallback?.Invoke();
    }
}