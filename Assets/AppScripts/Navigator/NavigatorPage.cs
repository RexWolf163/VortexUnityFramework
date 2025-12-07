using System;
using System.Collections.Generic;
using Vortex.Core.Extensions.LogicExtensions;

namespace AppScripts.Navigator
{
    public class NavigatorPage
    {
        public NavigatorPage(string key, string name, string backPage, byte[][] photos,
            byte[] sсheme, int photoWidth = 920)
        {
            Key = key;
            Name = name;
            BackPage = backPage;
            Photos = photos;
            Content = Array.Empty<string>();
            Scheme = sсheme;
            PhotoWidth = photoWidth;
        }

        public NavigatorPage(string key, string name, string backPage, byte[][] photos,
            string[] content, int photoWidth = 920)
        {
            Key = key;
            Name = name;
            BackPage = backPage;
            Photos = photos;
            Content = content;
            PhotoWidth = photoWidth;
            Scheme = null;
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
        public byte[][] Photos { get; }

        /// <summary>
        /// Переходы на другие страницы с этой
        /// </summary>
        public List<string> Links { get; } = new();

        /// <summary>
        /// Наполнение страницы
        /// </summary>
        public string[] Content { get; }

        /// <summary>
        /// Картинка-контент
        /// </summary>
        public byte[] Scheme { get; }

        public int PhotoWidth { get; }

        public void SetLink(string link) => Links.AddOnce(link);
    }
}