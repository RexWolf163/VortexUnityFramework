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
        /// <summary>
        /// Задержка вывода следующей буквы
        /// </summary>
        [SerializeField, Range(0, 0.2f)] private float letterDelay = 0.03f;

        /// <summary>
        /// Задержка после знака препинания
        /// </summary>
        [SerializeField, Range(0, 1f)] private float controlDelay = 0.3f;

        private Coroutine typingCoroutine;

        /// <summary>
        /// Кеш целевого текста
        /// </summary>
        private string _fullText;

        public override void PutData(string text)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            typingCoroutine = StartCoroutine(TypeText(text));
        }

        /// <summary>
        /// Корутина вывода текста
        ///
        /// Логика вывода:
        /// Если Печатается буква или символ \n, то перед следующей буквой выдерживается пауза letterDelay
        /// Если печатается символ пунктуации - паузы нет
        /// Если печатается буква или символ \n после знака пунктуации - выдерживается пауза controlDelay
        /// </summary>
        /// <param name="fullText"></param>
        /// <returns></returns>
        private IEnumerator TypeText(string fullText)
        {
            _fullText = fullText;
            SetText(string.Empty);

            bool isDelay = false;
            foreach (char c in _fullText)
            {
                if (!IsPunctuationChar(c))
                {
                    if (isDelay)
                    {
                        isDelay = false;
                        yield return new WaitForSeconds(controlDelay);
                    }
                    else
                        yield return new WaitForSeconds(letterDelay);
                }

                AppendChar(c);

                if (IsPunctuationChar(c))
                    isDelay = true;
            }

            typingCoroutine = null;
        }

        /// <summary>
        /// Символы, которые печатаются мгновенно
        /// (знаки пунктуации, спецсимволы)
        /// </summary>
        private bool IsPunctuationChar(char c) => char.IsPunctuation(c) || char.IsSymbol(c);

        protected override void OnDestroy()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            base.OnDestroy();
        }
#if UNITY_EDITOR
        /// <summary>
        /// Перезапуск вывода текста для тестирования
        /// </summary>
        [Button]
        private void Retype() => PutData(_fullText);
#endif
    }
}