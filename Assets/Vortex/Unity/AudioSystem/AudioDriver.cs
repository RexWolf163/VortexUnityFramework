using System;
using System.Collections.Generic;
using Vortex.Core.AudioSystem;
using Vortex.Core.AudioSystem.Bus;
using Vortex.Core.AudioSystem.Model;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions;
using Vortex.Unity.AudioSystem.Handlers;
using AudioSettings = Vortex.Core.AudioSystem.Model.AudioSettings;

namespace Vortex.Unity.AudioSystem
{
    public partial class AudioDriver : Singleton<AudioDriver>, IDriver
    {
        private const string SaveKey = "AudioSettings";
        public event Action OnInit;

        private static SortedDictionary<string, SoundSample> _indexSound;
        private static SortedDictionary<string, MusicSample> _indexMusic;
        private static AudioSettings _settings;

        private static AudioHandler _audioHandler;

        public void Init()
        {
            Database.OnInit += OnDatabaseInit;
            Audio.OnSettingsChanged += SaveSettings;
        }

        /// <summary>
        /// Выставление хэндлера
        /// </summary>
        /// <param name="audioHandler"></param>
        public static void SetHandler(AudioHandler audioHandler) => _audioHandler = audioHandler;

        public void Destroy()
        {
            Database.OnInit -= OnDatabaseInit;
            Audio.OnSettingsChanged -= SaveSettings;
        }

        /// <summary>
        /// Заполнение индексов
        /// </summary>
        private void OnDatabaseInit()
        {
            Database.OnInit -= OnDatabaseInit;
            var list = Database.GetRecords<SoundSample>();
            _indexSound.Clear();
            foreach (var soundSample in list)
                _indexSound.AddNew(soundSample.GuidPreset, soundSample);
            var list2 = Database.GetRecords<MusicSample>();
            _indexMusic.Clear();
            foreach (var soundSample in list2)
                _indexMusic.AddNew(soundSample.GuidPreset, soundSample);
            OnInit?.Invoke();
        }

        public void SetLinks(SortedDictionary<string, SoundSample> indexSound,
            SortedDictionary<string, MusicSample> indexMusic, AudioSettings settings)
        {
            _indexSound = indexSound;
            _indexMusic = indexMusic;
            _settings = settings;
        }

        public void PlaySound(SoundSample sample)
        {
        }

        public void PlayMusic(MusicSample sample)
        {
            throw new NotImplementedException();
        }

        public void StopMusic()
        {
            throw new NotImplementedException();
        }

        private static void SaveSettings()
        {
        }

        private static void LoadSettings()
        {
        }
    }
}