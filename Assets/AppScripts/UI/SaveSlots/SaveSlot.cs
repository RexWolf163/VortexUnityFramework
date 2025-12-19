using System.Linq;
using AppScripts.Player;
using UnityEngine;
using Vortex.Core.LocalizationSystem;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Unity.LocalizationSystem;
using Vortex.Unity.UI.UIComponents;

namespace AppScripts.UI.SaveSlots
{
    public class SaveSlot : MonoBehaviour
    {
        [SerializeField, LocalizationKey] private string titleFormat;
        [SerializeField] private UIComponent button;
        [SerializeField] private UIComponent title;
        [SerializeField] private UIComponent summary;

        [SerializeField, Range(0, 10)] private int numberSlot;

        [SerializeField, LocalizationKey] private string newGameText;

        private string _key;

        private void OnEnable()
        {
            button.SetAction(ChangeSaveSlot);
            title.SetText(string.Format(Localization.GetTranslate(titleFormat), numberSlot + 1));
            var index = SaveController.GetIndex();
            var keys = index.Keys.ToArray();
            if (keys.Length <= numberSlot)
            {
                this.summary.SetText(newGameText.Translate());
                return;
            }

            _key = keys[numberSlot];
            var summaryData = index[_key];
            title.SetText(summaryData.Date.ToString("HH:mm dd.MM.yyyy"));
        }

        private void OnDisable()
        {
        }

        private void ChangeSaveSlot()
        {
            PlayerController.SetSaveSlot(_key);
        }
    }
}