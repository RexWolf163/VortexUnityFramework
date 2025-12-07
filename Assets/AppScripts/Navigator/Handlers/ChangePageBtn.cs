using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Core.Extensions.DefaultEnums;
using Vortex.Unity.UI.UIComponents;

namespace AppScripts.Navigator.Handlers
{
    /// <summary>
    /// Хэндлер для привязки кнопок к переходам на конкретную страницу
    /// Можно настроить для работы с фиксированной страницей, а можно инициализировать через Init
    /// </summary>
    public class ChangePageBtn : MonoBehaviour
    {
        [SerializeField] private UIComponent btnComponent;

        [ValueDropdown("GetList")] [SerializeField] [OnValueChanged("OnChangePageID")]
        private string pageKey;

        private void OnEnable()
        {
            //Реинициализация текущими данными
            Init(pageKey);
            NavigatorController.OnChangePage += CheckPage;
            CheckPage();
        }

        private void OnDisable()
        {
            NavigatorController.OnChangePage -= CheckPage;
        }

        public void Init(string pageID)
        {
            pageKey = pageID;
            var page = NavigatorController.GetPageData(pageKey);
            if (page != null)
                btnComponent.SetTextAll(page.Name);

            btnComponent.SetAction(ChangePage);
        }

        /// <summary>
        /// Вызов смены страницы
        /// </summary>
        private void ChangePage() => NavigatorController.Page(pageKey);

        /// <summary>
        /// Проверка, активна ли связанная страница
        /// </summary>
        private void CheckPage()
        {
            btnComponent.SetSwitcher(NavigatorController.GetCurrentPage() == pageKey
                ? SwitcherState.On
                : SwitcherState.Off);
        }

#if UNITY_EDITOR
        private List<string> GetList()
        {
            var result = new List<string>();
            result.Add("");
            result.AddRange(NavigatorController.GetPagesList());
            return result;
        }

        private void OnChangePageID()
        {
            if (pageKey.IsNullOrWhitespace() || NavigatorController.GetPageData(pageKey) == null)
                return;
            Init(pageKey);
        }
#endif
    }
}