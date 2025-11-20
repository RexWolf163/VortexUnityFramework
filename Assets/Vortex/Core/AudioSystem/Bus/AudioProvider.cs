using System;
using System.Collections.Generic;
using Vortex.Core.AudioSystem.Model;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.AudioSystem.Bus
{
    public class AudioProvider : SystemController<AudioProvider, IDriver>
    {
        #region Params

        private static readonly SortedDictionary<string, IAudioSample> IndexSound = new();

        private static readonly SortedDictionary<string, IAudioSample> IndexMusic = new();

        public static AudioSettings Settings { get; } = new();

        #endregion

        #region Events

        /// <summary>
        /// Были изменены настройки звука
        /// </summary>
        public static event Action OnSettingsChanged;

        #endregion

        protected override void OnDriverConnect()
        {
            Driver.SetLinks(IndexSound, IndexMusic, Settings);
        }

        protected override void OnDriverDisonnect()
        {
        }

        /// <summary>
        /// Включить/выключить звуки
        /// </summary>
        /// <param name="soundOn"></param>
        public static void SetSoundState(bool soundOn)
        {
            Settings.SoundOn = soundOn;
            OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Включить/выключить музыку
        /// </summary>
        /// <param name="musicOn"></param>
        public static void SetMusicState(bool musicOn)
        {
            Settings.MusicOn = musicOn;
            OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Изменить громкость звуков
        /// </summary>
        /// <param name="value">значение от 0 до 1</param>
        public static void SetSoundVolume(float value)
        {
            Settings.SoundVolume = value;
            OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Изменить громкость музыки
        /// </summary>
        /// <param name="value">значение от 0 до 1</param>
        public static void SetMusicVolume(float value)
        {
            Settings.MusicVolume = value;
            OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Получить сэмпл звука
        /// </summary>
        /// <returns></returns>
        public static IAudioSample GetSample(string guid)
        {
            if (IndexSound.TryGetValue(guid, out var soundSample))
                return soundSample;
            if (IndexMusic.TryGetValue(guid, out var musicSample))
                return musicSample;

            Log.Print(new LogData(LogLevel.Error, $"Sample #{guid} not found.", "AudioPlayer"));
            return null;
        }
    }
}