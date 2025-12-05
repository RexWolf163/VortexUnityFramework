using System;
using UnityEngine;
using Vortex.Unity.AudioSystem.Model;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.LocalizationSystem;

namespace AppSdk.Comics.Database
{
    /// <summary>
    /// Картинка комикса
    /// Содержит музыку, текст и картинку
    /// </summary>
    [Serializable]
    public class ComicsStage
    {
        [SerializeField, LocalizationKey] private string text;

        [SerializeField] private Sprite picture;

        [SerializeField, DbRecord(typeof(Music))]
        private string musicId;

        public string MusicId
        {
            get => musicId;
            internal set => musicId = value;
        }

        public Sprite Picture
        {
            get => picture;
            internal set => picture = value;
        }

        public string Text
        {
            get => text;
            internal set => text = value;
        }
    }
}