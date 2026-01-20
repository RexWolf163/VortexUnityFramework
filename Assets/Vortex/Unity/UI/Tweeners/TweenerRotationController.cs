using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.UI.Tweeners
{
    /// <summary>
    /// Анимация вращения
    /// </summary>
    public class TweenerRotationController : TweenerBase
    {
        [SerializeField, Tooltip("Режим проигрывания")]
        private RotateMode mode;

        /// <summary>
        /// Положение стартовое (скрыто)
        /// </summary>
        [SerializeField, Tooltip("Положение при скрытие")]
        private Vector3 start;

        /// <summary>
        /// Положение финальное (показано)
        /// </summary>
        [SerializeField, Tooltip("Положение при показе")]
        private Vector3 end;

        /// <summary>
        /// длительность перехода
        /// </summary>
        [SerializeField, Range(0.1f, 2f)] private float duration = 0.3f;

        private RectTransform rect;
        private Tween tween;

        private void Awake() => DOTween.Init();

        private void OnDestroy() => tween.Kill();


        private RectTransform Rect
        {
            get
            {
                if (rect == null)
                    rect = transform.GetComponent<RectTransform>();
                return rect;
            }
        }

        [Button]
        public override void Forward(bool skip = false)
        {
            if (skip)
            {
                Rect.localEulerAngles = end;
                return;
            }

            tween.Kill();
            tween = transform.DOLocalRotate(end, duration, mode);
        }

        [Button]
        public override void Back(bool skip = false)
        {
            if (skip)
            {
                Rect.localEulerAngles = start;
                return;
            }

            tween.Kill();
            tween = transform.DOLocalRotate(start, duration, mode);
        }

        public override void Pulse()
        {
            tween.Kill();
            tween = transform.DOLocalRotate(end, duration, mode)
                .OnComplete(() => Back());
        }
    }
}