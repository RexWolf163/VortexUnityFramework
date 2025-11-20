using System;
using System.Linq;
using System.Reflection;
using Object = System.Object;

namespace Vortex.Core.System.Abstractions.SystemControllers
{
    public static class SystemModelController
    {
        /// <summary>
        /// Заполняет свои поля имеющимися в объекте данными
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool CopyFrom(this SystemModel target, Object source)
        {
            try
            {
                var properties = source.GetReadablePropertiesList();
                var modelType = target.GetType();
                foreach (var property in properties)
                {
                    var prop = modelType.GetProperty(property.Name);
                    if (prop == null || !prop.CanWrite)
                        continue;

                    prop.SetValue(target, property.GetValue(source));
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Получить список доступных для чтения параметром объекта
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static PropertyInfo[] GetReadablePropertiesList(this Object source) =>
            source.GetType()
                .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.CanWrite).ToArray();
    }
}