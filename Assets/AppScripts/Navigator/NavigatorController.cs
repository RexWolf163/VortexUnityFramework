using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.Enums;

namespace AppScripts.Navigator
{
    /// <summary>
    /// Контроллер навигации по информационным страницам
    /// </summary>
    public partial class NavigatorController : Singleton<NavigatorController>
    {
        /// <summary>
        /// Событие перехода на новую страницу
        /// </summary>
        public static event Action OnChangePage;

        /// <summary>
        /// Реестр страниц
        /// </summary>
        private static SortedDictionary<string, NavigatorPage> _pages = new();

        /// <summary>
        /// Текущая страничка
        /// </summary>
        private static string currentPage = string.Empty;

        /// <summary>
        /// Название стартовой страницы.
        /// Берется первое из списка страниц
        /// </summary>
        private static string homePage;

        /// <summary>
        /// Переход на основную страницу навигатора
        /// </summary>
        public static void Home()
        {
            currentPage = homePage;
            OnChangePage?.Invoke();
        }

        /// <summary>
        /// Проверка, является ли страница стартовой
        /// </summary>
        /// <returns></returns>
        public static bool IsHome() => currentPage.IsNullOrWhitespace() || currentPage == homePage;

        /// <summary>
        /// Переход на страницу
        /// </summary>
        /// <param name="pageName"></param>
        public static void Page(string pageName)
        {
            if (!_pages.ContainsKey(pageName))
            {
                Debug.LogError($"There is no page with name {pageName}");
                return;
            }

            currentPage = pageName;
            OnChangePage?.Invoke();
        }

        /// <summary>
        /// Возвращает ID текущей страницы
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPage() => currentPage;

        /// <summary>
        /// Возвращает данные страницы с указанным ID
        /// </summary>
        /// <param name="pageKey"></param>
        /// <returns></returns>
        public static NavigatorPage GetPageData(string pageKey)
        {
            if (pageKey.IsNullOrWhitespace())
                return _pages[homePage];
            if (!_pages.TryGetValue(pageKey, out var page) && App.GetState() == AppStates.Running)
                Debug.LogError($"There is no page with name {pageKey}");

            return page;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Возвращает перечень ключей страниц
        /// </summary>
        /// <returns></returns>
        public static string[] GetPagesList()
        {
            if (_pages == null || _pages.Count == 0)
                return new string[0];
            return _pages.Keys.ToArray();
        }
#endif
    }
}