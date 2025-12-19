using AppScripts.Narrative;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.PoolSystem;
using Vortex.Unity.UI.UIComponents;

namespace AppScripts.NarrativeNavigator.View
{
    public class NarrativeDialogueUI : MonoBehaviour
    {
        /// <summary>
        /// Вывод текста
        /// </summary>
        [SerializeField] private UIComponent text;

        /// <summary>
        /// Кнопки выбора ответа/действия
        /// </summary>
        [SerializeField] private Pool buttons;

        private void OnEnable()
        {
            NarrativeController.OnDialogueStateChanged += Refresh;
            TimeController.Accumulate(Init, this);
        }

        private void OnDisable()
        {
            NarrativeController.OnDialogueStateChanged -= Refresh;
            TimeController.RemoveCall(this);
        }

        [Button]
        private void Refresh()
        {
            var dialogue = NarrativeController.GetDialogueData();
            text.SetText(dialogue?.Text ?? "");
        }

        private void Init()
        {
            text.SetText("");
            Refresh();
        }
    }
}