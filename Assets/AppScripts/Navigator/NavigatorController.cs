using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.System.Abstractions;

namespace AppScripts.Navigator
{
    public class NavigatorController : SystemController<NavigatorController, NavigatorDriver>
    {
        public static event Action OnChangePage;

        private static SortedDictionary<string, NavigatorPage> _pages = new();

        /// <summary>
        /// Текущая страничка
        /// </summary>
        private static string currentPage;

        /// <summary>
        /// Переход на основную страницу навигатора
        /// </summary>
        public static void Home()
        {
            currentPage = "";
            OnChangePage?.Invoke();
        }

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

        protected override void OnDriverConnect() => Driver.SetLink(_pages);

        protected override void OnDriverDisonnect()
        {
        }
    }
}