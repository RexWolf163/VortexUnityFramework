using System;
using UnityEngine;

namespace AppScripts.Navigator
{
    public class NavigatorPage
    {
        public NavigatorPage(string key, string name, string backPage, string[] links, Texture2D[] photos,
            Texture2D sсheme)
        {
            Key = key;
            Name = name;
            BackPage = backPage;
            Links = links;
            Photos = photos;
            Content = Array.Empty<string>();
            Sсheme = sсheme;
        }

        public NavigatorPage(string key, string name, string backPage, string[] links, Texture2D[] photos,
            string[] content)
        {
            Key = key;
            Name = name;
            BackPage = backPage;
            Links = links;
            Photos = photos;
            Content = content;
            Sсheme = null;
        }

        /// <summary>
        /// Идентификатор страницы
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Название страницы
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Предыдущая страница (родитель)
        /// </summary>
        public string BackPage { get; }

        /// <summary>
        /// Фотографии
        /// </summary>
        public Texture2D[] Photos { get; }

        /// <summary>
        /// Переходы на другие страницы с этой
        /// </summary>
        public string[] Links { get; }

        /// <summary>
        /// Наполнение страницы
        /// </summary>
        public string[] Content { get; }

        /// <summary>
        /// Картинка-контент
        /// </summary>
        public Texture2D Sсheme { get; }
    }
}