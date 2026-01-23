namespace Vortex.Core.System.Abstractions
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance => _instance ??= new T();

        protected static void Dispose() => _instance = null;
    }
}