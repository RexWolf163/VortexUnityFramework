using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.UI.UIComponents.Parts
{
    /// <summary>
    /// UI-компонент, который выводит текст с эффектом печати.
    /// Текст появляется посимвольно с задержками, учитывающими пунктуацию
    /// и специальные символы.
    /// </summary>
    public class UIComponentTextTypewriter : UIComponentText
    {
        [SerializeField] private float letterDelay = 0.03f;
        [SerializeField] private float controlDelay = 0.3f;

        private Coroutine typingCoroutine;

        public override void PutData(string text)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            typingCoroutine = StartCoroutine(TypeText(text));
        }

        private IEnumerator TypeText(string fullText)
        {
            SetText(string.Empty);

            bool isDelay = false;
            foreach (char c in fullText)
            {
                if (isDelay && !IsInstantChar(c))
                {
                    isDelay = false;
                    yield return new WaitForSeconds(controlDelay);
                }

                if (IsControlChar(c))
                {
                    isDelay = true;
                    continue;
                }
                
                AppendChar(c);

                if (!IsInstantChar(c))
                {
                    yield return new WaitForSeconds(letterDelay);
                    continue;
                }
                isDelay = true;
            }

            typingCoroutine = null;
        }
        
        /// <summary>
        /// Символы, которые печатаются мгновенно
        /// (знаки пунктуации, спецсимволы)
        /// </summary>
        private bool IsInstantChar(char c) => char.IsPunctuation(c) || char.IsSymbol(c);
        
        /// <summary>
        /// Управляющие символы (перенос строки, табуляция и т.д.),
        /// кроме пробела
        /// </summary>
        private bool IsControlChar(char c) => char.IsControl(c) && c != ' ';

        protected override void OnDestroy()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            base.OnDestroy();
        }
        #if UNITY_EDITOR
        [Button]
        private void Retype()
        {
            PutData(GetValue());
        }
        #endif
    }
}