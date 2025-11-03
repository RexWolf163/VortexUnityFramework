using System.Collections.Generic;
using Vortex.Core.AudioSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.AudioSystem
{
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Передача линка на реестр
        /// </summary>
        /// <param name="indexSound">Ссылка на реестр сэмплов звуков</param>
        /// <param name="indexMusic">Ссылка на реестр сэмплов музыки</param>
        /// <param name="settings">Ссылка на базовые настройки воспроизведения</param>
        public void SetLinks(SortedDictionary<string, SoundSample> indexSound,
            SortedDictionary<string, MusicSample> indexMusic, AudioSettings settings);

        /// <summary>
        /// Проиграть указанный звук
        /// </summary>
        /// <param name="sample"></param>
        public void PlaySound(SoundSample sample);

        /// <summary>
        /// Запустить указанную музыку
        /// </summary>
        /// <param name="sample"></param>
        public void PlayMusic(MusicSample sample);

        /// <summary>
        /// Остановить воспроизведение музыки
        /// </summary>
        public void StopMusic();
    }
}