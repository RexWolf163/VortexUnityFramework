using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;

namespace AppScripts.Navigator
{
    public class NavigatorPage
    {
        public NavigatorPage(string key, string name, string backPage, byte[][] photos,
            byte[] sсheme)
        {
            Key = key;
            Name = name;
            BackPage = backPage;
            _photosRaw = photos;
            Content = Array.Empty<string>();
            _schemeRaw = sсheme;
        }

        public NavigatorPage(string key, string name, string backPage, byte[][] photos,
            string[] content)
        {
            Key = key;
            Name = name;
            BackPage = backPage;
            _photosRaw = photos;
            Content = content;
            _schemeRaw = null;
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
        public Texture2D[] Photos
        {
            get
            {
                if (_photos != null)
                    return _photos;
                var c = _photosRaw.Length;
                _photos = new Texture2D[c];
                for (int i = 0; i < c; i++)
                {
                    _photos[i] = new Texture2D(1, 1);
                    if (!_photos[i].LoadImage(_photosRaw[i]))
                        Debug.LogError($"Broken image {i} for {Name}");
                    else
                        _photos[i].Apply(); //на всякий случай
                }

                return _photos;
            }
        }

        private Texture2D[] _photos;

        private readonly byte[][] _photosRaw;

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
        public Texture2D Sсheme
        {
            get
            {
                if (_schemeRaw == null)
                    return null;
                if (_scheme != null)
                    return _scheme;
                _scheme = new Texture2D(1, 1);
                if (!_scheme.LoadImage(_schemeRaw))
                    Debug.LogError($"Broken scheme for {Name}");
                else
                    _scheme.Apply(); //на всякий случай
                return _scheme;
            }
        }

        private Texture2D _scheme;
        private readonly byte[] _schemeRaw;

        public void SetLink(string link) => Links.AddOnce(link);
    }
}