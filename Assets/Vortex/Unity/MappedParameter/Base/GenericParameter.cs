using System;
using UnityEngine;

namespace Vortex.Unity.MappedParameter
{
    /// <summary>
    /// Динамически определяемый при настройке параметр, связанный с другим параметром некой логикой
    /// по принципу однонаправленного списка.
    /// Родитель параметра определяется при помощи управляющего контроллера
    /// Логика определяется управляющим контроллером
    /// </summary>
    [Serializable]
    public class GenericParameter
    {
        /// <summary>
        /// Название параметра
        /// </summary>
        [field: SerializeField]
        public string Name { get; internal set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        [field: SerializeField]
        public int Value { get; internal set; }

        public GenericParameter(string name) => Name = name;

        /// <summary>
        /// Установить новое значение
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value) => Value = value;
    }
}