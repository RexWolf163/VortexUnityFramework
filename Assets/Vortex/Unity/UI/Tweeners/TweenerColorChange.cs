using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Vortex.Unity.UI.Tweeners
{
    /// <summary>
    /// Твинер изменения цвета
    /// </summary>
    public class TweenerColorChange : TweenerBase
    {
        /// <summary>
        /// Конечный цвет
        /// </summary>
        [SerializeField] [HorizontalGroup("h1")]
        private Color startColor;

        /// <summary>
        /// Начальный цвет
        /// </summary>
        [SerializeField] [HorizontalGroup("h2")]
        private Color endColor;

        /// <summary>
        /// Целевое изображение
        /// </summary>
        [SerializeField] private Image image;

        /// <summary>
        /// длительность перехода
        /// </summary>
        [SerializeField, Range(0.1f, 2f)] private float duration = 0.2f;

        [SerializeField] private bool disableOnStart = true;
        [SerializeField] private bool disableOnEnd;

        private Tween tween;

        private void Awake()
        {
            DOTween.Init(image);
        }

        private void OnDestroy()
        {
            tween.Kill();
        }

        public override void Forward(bool skip = false)
        {
            if (skip)
            {
                image.color = endColor;
                if (disableOnEnd)
                    image.gameObject.SetActive(false);
                return;
            }

            image.gameObject.SetActive(true);
            tween.Kill();
            tween = DOTween.To(() => image.color, color => image.color = color, endColor, duration)
                .OnComplete(() =>
                {
                    if (disableOnEnd)
                        image.gameObject.SetActive(false);
                });
        }

        public override void Back(bool skip = false)
        {
            if (skip)
            {
                image.color = startColor;
                if (disableOnStart)
                    image.gameObject.SetActive(false);
                return;
            }

            image.gameObject.SetActive(true);
            tween.Kill();
            tween = DOTween.To(() => image.color, color => image.color = color, startColor, duration)
                .OnComplete(() =>
                {
                    if (disableOnStart)
                        image.gameObject.SetActive(false);
                });
        }

#if UNITY_EDITOR
        [ShowIf("@image != null")]
        [Button("Get from...")]
        [GUIColor("@Color.green")]
        [HorizontalGroup("h1")]
        private void SetCurrentStart() => startColor = image.color;

        [ShowIf("@image != null")]
        [Button("Get from...")]
        [GUIColor("@Color.green")]
        [HorizontalGroup("h2")]
        private void SetCurrentEnd() => endColor = image.color;

#endif
    }
}