using System;
using AppSdk.Comics.Database;
using DB = Vortex.Core.DatabaseSystem.Bus.Database;

namespace AppSdk.Comics
{
    /// <summary>
    /// Контроллер управления комиксами-заставками
    /// Может показываться только 1 комикс одномоментно
    /// </summary>
    public static class ComicsController
    {
        public static event Action OnCallComics;
        public static event Action OnStopComics;

        private static ComicsData _currentComics;

        /// <summary>
        /// Запуск комикса по ID  в БД
        /// </summary>
        /// <param name="comicsId"></param>
        public static void StartComics(string comicsId)
        {
            _currentComics = DB.GetRecord<ComicsData>(comicsId);
            OnCallComics?.Invoke();
        }

        /// <summary>
        /// Остановить (закончить) комикс
        /// Комикс будет помечен как "показанный"
        /// </summary>
        public static void StopComics()
        {
            if (_currentComics == null)
                return;
            _currentComics.WasShowed = true;
            _currentComics = null;
            OnStopComics?.Invoke();
        }

        /// <summary>
        /// Возвращает данные текущего комикса под показ.
        /// Может показываться только 1 комикс одномоментно
        /// </summary>
        /// <returns></returns>
        public static ComicsData GetCurrentComics() => _currentComics;
    }
}