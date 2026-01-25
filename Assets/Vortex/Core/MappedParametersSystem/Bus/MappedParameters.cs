using System;
using System.Collections.Generic;
using Vortex.Core.MappedParametersSystem.Base;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.MappedParametersSystem.Bus
{
    /// <summary>
    /// Система доступа к схемам связанных параметров
    /// </summary>
    public class MappedParameters : SystemController<MappedParameters, IDriverMappedParameters>
    {
        private static Dictionary<string, IMappedParametersGroup> _parametersMaps = new();

        protected override void OnDriverConnect()
        {
            Driver.SetIndex(_parametersMaps);
        }

        protected override void OnDriverDisonnect()
        {
        }

        /// <summary>
        /// Получить перечень параметров для карты указанного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetParameters<T>() => GetParameters(typeof(T));

        /// <summary>
        /// Получить перечень параметров для карты указанного типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetParameters(Type type)
        {
            return type.AssemblyQualifiedName == null
                ? Array.Empty<string>()
                : GetParameters(type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Получить перечень параметров для карты указанного типа (AssemblyQualifiedName)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string[] GetParameters(string name) =>
            _parametersMaps.TryGetValue(name, out var result)
                ? result.GetParametersList()
                : Array.Empty<string>();
    }
}