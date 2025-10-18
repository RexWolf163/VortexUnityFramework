using System;
using System.Collections.Generic;

namespace Vortex.Database.Model.Record
{
    public abstract partial class Record
    {
        /// <summary>
        /// Получить GUID
        /// </summary>
        /// <returns></returns>
        public partial string GetGuid();

        /// <summary>
        /// Получить наименование
        /// </summary>
        /// <returns></returns>
        public partial string GetName();

        /// <summary>
        /// Получить описание
        /// </summary>
        /// <returns></returns>
        public partial string GetDescription();

        public string GetCsvTitle()
        {
            var fields = GetType().GetFields();
            var arResult = new List<string>();
            foreach (var field in fields)
                arResult.Add($"\"{field.Name}\"");
            return String.Join(",", arResult);
        }

        public string ToCsvString()
        {
            var fields = GetType().GetFields();
            var arResult = new List<string>();
            foreach (var field in fields)
                arResult.Add($"\"{field.GetValue(this)}\"");
            var result = String.Join(",", arResult);
            return result;
        }

        public bool FromCsvString(string csvString)
        {
            //TODO
            return false;
        }

#if UNITY_EDITOR
        protected Record() => MakeGuid();

        /// <summary>
        /// Формирование уникального GUID на основании доступных данных
        /// </summary>
        /// <returns></returns>
        private partial void MakeGuid();
    }
#endif
}