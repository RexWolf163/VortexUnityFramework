using System.Collections.Generic;

namespace Vortex.Core.Extensions.LogicExtensions
{
    public static class ListExt
    {
        /// <summary>
        /// Проверить на наличие в списке указанного значения
        /// Если значение не найдено - добавить его в список
        /// </summary>
        /// <param name="owner">Список</param>
        /// <param name="data">Вставляемое значение</param>
        /// <typeparam name="T">Тип списка</typeparam>
        public static void AddOnce<T>(this List<T> owner, T data)
        {
            if (owner.Contains(data))
                return;
            owner.Add(data);
        }
    }
}