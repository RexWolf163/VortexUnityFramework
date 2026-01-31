using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.UI.Tweeners
{
    public abstract class TweenerBase : MonoBehaviour
    {
        private void OnEnable() => Back(true);

        /// <summary>
        /// Анимация в прямом направлении
        /// </summary>
        /// <param name="skip">Мгновенный переход</param>
        [Button]
        public abstract void Forward(bool skip = false);

        /// <summary>
        /// Анимация в обратном направлении
        /// </summary>
        /// <param name="skip">Мгновенный переход</param>
        [Button]
        public abstract void Back(bool skip = false);

        /// <summary>
        /// Анимация туда-сюда
        /// </summary>
        [Button]
        public abstract void Pulse();
    }
}