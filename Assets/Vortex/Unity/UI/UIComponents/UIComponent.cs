using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Vortex.Unity.UI.UIComponents.Parts;

namespace Vortex.Unity.UI.UIComponents
{
    public class UIComponent : MonoBehaviour
    {
        /// <summary>
        /// Перечень текстовых полей компонента
        /// </summary>
        [SerializeField, HideIf("EmptyTexts"), HorizontalGroup("Link"),
         VerticalGroup("Link/Components")]
        private UIComponentText[] uiComponentTexts;

        /// <summary>
        /// Перечень кнопок компонента
        /// </summary>
        [SerializeField, HideIf("EmptyButtons"), HorizontalGroup("Link"),
         VerticalGroup("Link/Components")]
        private UIComponentButton[] uiComponentButtons;

        /// <summary>
        /// Перечень графических элементов компонента
        /// </summary>
        [SerializeField, HideIf("EmptyGraphics"), HorizontalGroup("Link"),
         VerticalGroup("Link/Components")]
        private UIComponentGraphic[] uiComponentGraphics;

        #region Private

        private void OnEnable()
        {
            /*
            var count = uiComponentTexts?.Length ?? 0;
            for (var i = 0; i < count; i++)
                SetText(String.Empty, i);

            count = uiComponentGraphics?.Length ?? 0;
            for (var i = 0; i < count; i++)
                SetSprite(null, i);
        */
        }

        #endregion

        #region Public

        public void PutData(UIComponentData data)
        {
            var count = uiComponentTexts?.Length ?? 0;
            for (var i = 0; i < count; i++)
            {
                var text = String.Empty;
                if (data.texts?.Length > i)
                    text = data.texts[i];
                SetText(text, i);
            }

            count = uiComponentButtons?.Length ?? 0;
            for (var i = 0; i < count; i++)
            {
                UnityAction action = null;
                if (data.actions?.Length > i)
                    action = data.actions[i];
                SetAction(action, i);
            }

            count = uiComponentGraphics?.Length ?? 0;
            for (var i = 0; i < count; i++)
            {
                Sprite sprite = null;
                if (data.sprites?.Length > i)
                    sprite = data.sprites[i];
                SetSprite(sprite, i);
            }
        }

        /// <summary>
        /// Упрощенное добавление экшена на кнопку
        /// </summary>
        /// <param name="text">Текст для внедрения в компонент</param>
        /// <param name="position">Номер части компонента</param>
        public void SetText(string text, int position = 0)
        {
            if (uiComponentTexts == null || uiComponentTexts.Length <= position)
            {
                Debug.LogError($"[UIComponent: {transform.name}] No UI components for this content]");
                return;
            }

            uiComponentTexts[position].PutData(text);
        }

        /// <summary>
        /// Упрощенное добавление экшена на кнопку
        /// </summary>
        /// <param name="action">Событие для внедрения в компонент</param>
        /// <param name="position">Номер части компонента</param>
        public void SetAction(UnityAction action, int position = 0)
        {
            if (uiComponentButtons == null || uiComponentButtons.Length <= position)
            {
                Debug.LogError($"[UIComponent: {transform.name}] No UI components for this content]");
                return;
            }

            uiComponentButtons[position].PutData(action);
        }

        /// <summary>
        /// Упрощенное добавление экшена на кнопку
        /// </summary>
        /// <param name="sprite">Событие для внедрения в компонент</param>
        /// <param name="position">Номер части компонента</param>
        public void SetSprite(Sprite sprite, int position = 0)
        {
            if (uiComponentGraphics == null || uiComponentGraphics.Length <= position)
            {
                Debug.LogError($"[UIComponent: {transform.name}] No UI components for this content]");
                return;
            }

            uiComponentGraphics[position].PutData(sprite);
        }

        #endregion

#if UNITY_EDITOR

        private bool EmptyTexts() => uiComponentTexts?.Length == 0;
        private bool EmptyButtons() => uiComponentButtons?.Length == 0;
        private bool EmptyGraphics() => uiComponentGraphics?.Length == 0;

        [Button(ButtonHeight = 50), GUIColor("GetInitColor"), HorizontalGroup("Link", Width = 40f)]
        private void Init()
        {
            uiComponentTexts = GetComponentsInChildren<UIComponentText>(true);
            uiComponentButtons = GetComponentsInChildren<UIComponentButton>(true);
            uiComponentGraphics = GetComponentsInChildren<UIComponentGraphic>(true);

            _testData.texts = new string[uiComponentTexts?.Length ?? 0];
            var count = uiComponentTexts?.Length ?? 0;
            for (var i = 0; i < count; i++)
            {
                var uiComponentText = uiComponentTexts[i];
                _testData.texts[i] = uiComponentText.GetValue();
            }

            _testData.sprites = new Sprite[uiComponentGraphics?.Length ?? 0];
            count = uiComponentGraphics?.Length ?? 0;
            for (var i = 0; i < count; i++)
            {
                var uiComponentGraphic = uiComponentGraphics[i];
                _testData.sprites[i] = uiComponentGraphic.GetValue();
            }
        }

        private Color GetInitColor()
        {
            var red = new Color(1, 0.3f, 0.3f, 1);
            var white = new Color(0.9f, 0.9f, 0.9f, 1);

            if (uiComponentButtons != null)
                foreach (var component in uiComponentButtons)
                    if (component == null)
                        return red;
            if (uiComponentTexts != null)
                foreach (var component in uiComponentTexts)
                    if (component == null)
                        return red;
            if (uiComponentGraphics != null)
                foreach (var component in uiComponentGraphics)
                    if (component == null)
                        return red;

            return (uiComponentButtons == null || uiComponentButtons.Length == 0)
                   && (uiComponentTexts == null || uiComponentTexts.Length == 0)
                   && (uiComponentGraphics == null || uiComponentGraphics.Length == 0)
                ? red
                : TestNotReady()
                    ? Color.green
                    : white;
        }

        [TitleGroup("Debug"), ShowInInspector, PropertyOrder(999)]
        private UIComponentData _testData;

        [Button, PropertyOrder(1000), HideIf("TestNotReady")]
        private void Test() => PutData(_testData);

        private bool TestNotReady() => _testData.sprites == null || _testData.texts == null;
#endif
    }
}