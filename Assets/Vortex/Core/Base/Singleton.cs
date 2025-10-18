namespace Vortex.Core.Base
{
    /// <summary>
    /// Класс-абстракция для синглотонов
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        protected static T Instance => _instance ??= new T();
    }
}