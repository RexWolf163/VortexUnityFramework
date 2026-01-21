using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;

namespace Vortex.Core.ComplexModelSystem
{
    /// <summary>
    /// Класс-основа для комплексной модели данных.
    /// То есть для модели, чья структура зависит от подключенных пакетов расширения
    /// </summary>
    /// <typeparam name="T">Интерфейс или базовый класс данных</typeparam>
    public abstract class ComplexModel<T> where T : class
    {
        /// <summary>
        /// Защита от реиспользования на одинаковый T
        /// </summary>
        private static Dictionary<Type, Type[]> cache = new();

        protected Dictionary<Type, T> Index = new();

        public ComplexModel()
        {
        }

        /// <summary>
        /// Инициализация новой модели структурой
        /// </summary>
        public void Init()
        {
            Index.Clear();

            if (cache.ContainsKey(typeof(T)))
            {
                var types = cache[typeof(T)];
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
                    Debug.LogException(ex);
                }
            }

            cache[typeof(T)] = Index.Keys.ToArray();
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
    }
}