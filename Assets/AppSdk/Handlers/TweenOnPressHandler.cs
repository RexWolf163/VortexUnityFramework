using UnityEngine;
using UnityEngine.EventSystems;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Tweeners;

namespace AppSdk.Handlers
{
    public class TweenOnPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TweenerBase[] tweeners;

        /// <summary>
        /// Время запуска реверса
        /// </summary>
        [SerializeField] private float duration;

        private void OnEnable()
        {
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        private void OnDisable() => TimeController.RemoveCall(this);

        public void OnPointerDown(PointerEventData eventData)
        {
            foreach (var tweener in tweeners)
                tweener.Forward();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            foreach (var tweener in tweeners)
                tweener.Back();
        }
    }
}