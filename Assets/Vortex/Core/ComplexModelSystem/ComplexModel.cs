using System;
using System.Collections.Generic;
using System.Linq;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;

namespace Vortex.Core.ComplexModelSystem
{
    /// <summary>
    /// Класс-основа для комплексной модели данных.
    /// То есть для модели, чья структура зависит от подключенных пакетов расширения
    /// </summary>
    /// <typeparam name="T">Интерфейс или базовый класс данных</typeparam>
    [Serializable]
    public abstract class ComplexModel<T> where T : class
    {
        /// <summary>
        /// Защита от реиспользования на одинаковый T
        /// </summary>
        private static readonly Dictionary<Type, Type[]> Cache = new();

        public Dictionary<Type, T> Index { get; protected set; } = new();

        /// <summary>
        /// Инициализация новой модели структурой
        /// </summary>
        public void Init()
        {
            Index.Clear();

            if (Cache.ContainsKey(typeof(T)))
            {
                var types = Cache[typeof(T)];
                foreach (var t in types)
                    Index.Add(t, Activator.CreateInstance(t) as T);

                return;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(t => !t.IsAbstract
                                    && !t.IsInterface
                                    && typeof(T).IsAssignableFrom(t)
                                    && t.GetConstructor(Type.EmptyTypes) != null);

                    foreach (var t in types)
                        Index.Add(t, Activator.CreateInstance(t) as T);
                }
                catch (Exception ex)
                {
                    Log.Print(LogLevel.Warning, ex.Message, this);
                }
            }

            Cache[typeof(T)] = Index.Keys.ToArray();
        }

        /// <summary>
        /// Возвращает значение указанного типа если оно есть в индексе
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <returns></returns>
        public TU Get<TU>() where TU : class, T
        {
            if (Index.TryGetValue(typeof(TU), out var instance))
                return instance as TU;

            Log.Print(LogLevel.Error,
                $"Incorrect data request. Model not have {typeof(TU).Name} data.",
                this);
            return null;
        }

        public abstract string Serialize();

        public abstract void Deserialize(string data);
    }
}