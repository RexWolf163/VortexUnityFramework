using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.UI.Tweeners
{
    /// <summary>
    /// Анимация масштабирования
    /// </summary>
    public class TweenerScaleController : TweenerBase
    {
        /// <summary>
        /// Размер стартовый (скрыто)
        /// </summary>
        [SerializeField, Tooltip("Размер при скрытие")]
        private Vector3 start = Vector3.zero;

        /// <summary>
        /// Размер финальный (показано)
        /// </summary>
        [SerializeField, Tooltip("Размер при показе")]
        private Vector3 end = Vector3.one;

        /// <summary>
        /// длительность перехода
        /// </summary>
        [SerializeField, Range(0.1f, 2f)] private float duration = 0.3f;

        /// <summary>
        /// Флаг, указвающий на необходимость сброса скейла перед анимацией (tween)
        /// </summary>
        [SerializeField] private bool _resetScaleBeforeTween;

        /// <summary>
        /// Функция плавности анимации вперед
        /// </summary>
        [SerializeField, Tooltip("Подсказки по существующим функциям можно посмотреть на easings.net")]
        private Ease _easeForward = Ease.InSine;

        /// <summary>
        /// Функция плавности анимации назад
        /// </summary>
        [SerializeField, Tooltip("Подсказки по существующим функциям можно посмотреть на easings.net")]
        private Ease _easeBack = Ease.OutSine;

        private Transform rect;
        private Tween tween;

        private Transform Rect
        {
            get
            {
                if (rect == null)
                    rect = transform.GetComponent<Transform>();
                return rect;
            }
        }

        [Button]
        public override void Forward(bool skip = false)
        {
            if (_resetScaleBeforeTween) Rect.localScale = start;
            if (skip)
            {
                Rect.localScale = end;
                return;
            }

            tween.Kill();
            tween = Rect.DOScale(end, duration).SetEase(_easeForward).OnComplete(() => Rect.localScale = end);
        }

        [Button]
        public override void Back(bool skip = false)
        {
            if (_resetScaleBeforeTween) Rect.localScale = end;
            if (skip)
            {
                Rect.localScale = start;
                return;
            }

            tween.Kill();
            tween = Rect.DOScale(start, duration).SetEase(_easeBack).OnComplete(() => Rect.localScale = start);
        }

        [Button]
        public override void Pulse()
        {
            tween.Kill();
            tween = Rect.DOScale(end, duration).SetEase(_easeForward).OnComplete(() =>
                Back());
        }

        private void Awake() => DOTween.Init();
    }
}