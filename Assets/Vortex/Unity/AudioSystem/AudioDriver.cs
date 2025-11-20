using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Core.AudioSystem;
using Vortex.Core.AudioSystem.Bus;
using Vortex.Core.AudioSystem.Model;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.AudioSystem.Handlers;
using Vortex.Unity.AudioSystem.Model;
using AudioSettings = Vortex.Core.AudioSystem.Model.AudioSettings;

namespace Vortex.Unity.AudioSystem
{
    public partial class AudioDriver : Singleton<AudioDriver>, IDriver
    {
        private const string SaveKey = "AudioSettings";
        public event Action OnInit;

        private static SortedDictionary<string, IAudioSample> _indexSound;
        private static SortedDictionary<string, IAudioSample> _indexMusic;
        private static AudioSettings _settings;

        private static AudioHandler _audioHandler;

        public void Init()
        {
            Database.OnInit += OnDatabaseInit;
            AudioProvider.OnSettingsChanged += SaveSettings;
        }

        /// <summary>
        /// Выставление хэндлера
        /// </summary>
        /// <param name="audioHandler"></param>
        public static void SetHandler(AudioHandler audioHandler) => _audioHandler = audioHandler;

        public void Destroy()
        {
            Database.OnInit -= OnDatabaseInit;
            AudioProvider.OnSettingsChanged -= SaveSettings;
            TimeController.RemoveCall(this);
        }

        /// <summary>
        /// Заполнение индексов
        /// </summary>
        private void OnDatabaseInit()
        {
            Database.OnInit -= OnDatabaseInit;

            _indexSound.Clear();
            var list = Database.GetRecords<Sound>();
            foreach (var record in list)
                _indexSound.AddNew(record.GuidPreset, record);

            _indexMusic.Clear();
            var list2 = Database.GetRecords<Music>();
            foreach (var record in list2)
                _indexMusic.AddNew(record.GuidPreset, record);

            OnInit?.Invoke();
        }

        public void SetLinks(SortedDictionary<string, IAudioSample> indexSound,
            SortedDictionary<string, IAudioSample> indexMusic, AudioSettings settings)
        {
            _indexSound = indexSound;
            _indexMusic = indexMusic;
            _settings = settings;
            LoadSettings();
        }

        private static void SaveSettings()
        {
            var save =
                $"{(_settings.MusicOn ? "Y" : "N")};{_settings.MusicVolume};{(_settings.SoundOn ? "Y" : "N")};{_settings.SoundVolume}";
            PlayerPrefs.SetString(SaveKey, save);
            PlayerPrefs.Save();
        }

        private static void LoadSettings()
        {
            var save = PlayerPrefs.GetString(SaveKey);
            if (save.IsNullOrWhitespace())
                return;
            var ar = save.Split(';');
            try
            {
                AudioProvider.SetMusicState(ar[0] == "Y");
                AudioProvider.SetMusicVolume(float.Parse(ar[1]));
                AudioProvider.SetSoundState(ar[2] == "Y");
                AudioProvider.SetSoundVolume(float.Parse(ar[3]));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}