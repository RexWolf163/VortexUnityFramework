using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Vortex.Unity.UI.Tweeners
{
    /// <summary>
    /// Твинер изменения цвета
    /// </summary>
    public class TweenerTextColorChange : TweenerBase
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
        [SerializeField] private Text text;

        /// <summary>
        /// длительность перехода
        /// </summary>
        [SerializeField, Range(0.1f, 2f)] private float duration = 0.2f;

        [SerializeField] private bool disableOnStart = true;
        [SerializeField] private bool disableOnEnd;

        private Tween tween;

        private void Awake() => DOTween.Init(text);

        private void OnDestroy() => tween.Kill();

        public override void Forward(bool skip = false)
        {
            if (skip)
            {
                text.color = endColor;
                if (disableOnEnd)
                    text.gameObject.SetActive(false);
                return;
            }

            text.gameObject.SetActive(true);
            tween.Kill();
            tween = DOTween.To(() => text.color, color => text.color = color, endColor, duration)
                .OnComplete(() =>
                {
                    if (disableOnEnd)
                        text.gameObject.SetActive(false);
                });
        }

        public override void Back(bool skip = false)
        {
            if (skip)
            {
                text.color = startColor;
                if (disableOnStart)
                    text.gameObject.SetActive(false);
                return;
            }

            text.gameObject.SetActive(true);
            tween.Kill();
            tween = DOTween.To(() => text.color, color => text.color = color, startColor, duration)
                .OnComplete(() =>
                {
                    if (disableOnStart)
                        text.gameObject.SetActive(false);
                });
        }

        public override void Pulse()
        {
            tween.Kill();
            tween = DOTween.To(() => text.color, color => text.color = color, endColor, duration)
                .OnComplete(() => Back());
        }

#if UNITY_EDITOR
        [ShowIf("@text != null")]
        [Button("Get from...")]
        [GUIColor("@Color.green")]
        [HorizontalGroup("h1")]
        private void SetCurrentStart() => startColor = text.color;

        [ShowIf("@text != null")]
        [Button("Get from...")]
        [GUIColor("@Color.green")]
        [HorizontalGroup("h2")]
        private void SetCurrentEnd() => endColor = text.color;

#endif
    }
}