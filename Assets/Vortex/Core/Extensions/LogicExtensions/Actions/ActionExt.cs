using System;
using System.Collections.Generic;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;

namespace Vortex.Core.Extensions.LogicExtensions.Actions
{
    /// <summary>
    /// Расширение класса Action для запуска без проверки существования подписок
    /// (Синтаксический сахар)
    /// </summary>
    public static class ActionExt
    {
        public static void Fire(this Action action) => action?.Invoke();

        public static void Fire<T1>(this Action<T1> action, T1 t1) => action?.Invoke(t1);

        public static void Fire<T1, T2>(this Action<T1, T2> action, T1 t1, T2 t2) =>
            action?.Invoke(t1, t2);

        public static void Fire<T1, T2, T3>(this Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) =>
            action?.Invoke(t1, t2, t3);

        public static void Fire<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4) =>
            action?.Invoke(t1, t2, t3, t4);

        public static void Fire<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4,
            T5 t5) =>
            action?.Invoke(t1, t2, t3, t4, t5);

        /// <summary>
        /// Обработка подписок по логике AND 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="returnOnZero">что вернуть если нет подписок</param>
        /// <returns></returns>
        public static bool FireAnd(this Func<bool> func, bool returnOnZero = true)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if (!(bool)@delegate.DynamicInvoke())
                    return false;

            return true;
        }

        /// <summary>
        /// Обработка подписок по логике OR
        /// Выполнены будут все подписки, не взирая на последовательность и результат
        /// </summary>
        /// <param name="func"></param>
        /// <param name="returnOnZero">что вернуть если нет подписок</param>
        /// <returns></returns>
        public static bool FireOr(this Func<bool> func, bool returnOnZero = true)
        {
            var result = false;
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if ((bool)@delegate.DynamicInvoke())
                    result = true;

            return result;
        }

        public static bool FireAnd<T1>(this Func<T1, bool> func, T1 arg1, bool returnOnZero = true)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if (!(bool)@delegate.DynamicInvoke(arg1))
                    return false;

            return true;
        }

        public static bool FireOr<T1>(this Func<T1, bool> func, T1 arg1, bool returnOnZero = true)
        {
            var result = false;
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if ((bool)@delegate.DynamicInvoke(arg1))
                    result = true;

            return result;
        }

        public static bool FireAnd<T1, T2>(this Func<T1, T2, bool> func, T1 arg1, T2 arg2, bool returnOnZero = true)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if (!(bool)@delegate.DynamicInvoke(arg1, arg2))
                    return false;

            return true;
        }

        public static bool FireOr<T1, T2>(this Func<T1, T2, bool> func, T1 arg1, T2 arg2, bool returnOnZero = true)
        {
            var result = false;
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if ((bool)@delegate.DynamicInvoke(arg1, arg2))
                    result = true;

            return result;
        }

        public static bool FireAnd<T1, T2, T3>(this Func<T1, T2, T3, bool> func, T1 arg1, T2 arg2, T3 arg3,
            bool returnOnZero = true)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if (!(bool)@delegate.DynamicInvoke(arg1, arg2, arg3))
                    return false;

            return true;
        }

        public static bool FireOr<T1, T2, T3>(this Func<T1, T2, T3, bool> func, T1 arg1, T2 arg2, T3 arg3,
            bool returnOnZero = true)
        {
            var result = false;
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if ((bool)@delegate.DynamicInvoke(arg1, arg2, arg3))
                    result = true;

            return result;
        }

        public static bool FireAnd<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            bool returnOnZero = true)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if (!(bool)@delegate.DynamicInvoke(arg1, arg2, arg3, arg4))
                    return false;

            return true;
        }

        public static bool FireOr<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            bool returnOnZero = true)
        {
            var result = false;
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if ((bool)@delegate.DynamicInvoke(arg1, arg2, arg3, arg4))
                    result = true;

            return result;
        }

        public static bool FireAnd<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
            bool returnOnZero = true)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if (!(bool)@delegate.DynamicInvoke(arg1, arg2, arg3, arg4, arg5))
                    return false;

            return true;
        }

        public static bool FireOr<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
            bool returnOnZero = true)
        {
            var result = false;
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            if (list.Length == 0)
                return returnOnZero;
            foreach (var @delegate in list)
                if ((bool)@delegate.DynamicInvoke(arg1, arg2, arg3, arg4, arg5))
                    result = true;

            return result;
        }

        /// <summary>
        /// Сбор параметров от подписчиков в единый массив
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T[] Accumulate<T>(this Func<T[]> func)
        {
            var result = new List<T>();
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            foreach (var @delegate in list)
            {
                var outData = @delegate.DynamicInvoke();
                result.AddRange((T[])outData);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Сбор параметров от подписчиков в единый массив
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T[] Accumulate<T>(this Func<T> func)
        {
            var result = new List<T>();
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            try
            {
                foreach (var @delegate in list)
                {
                    var outData = @delegate.DynamicInvoke();
                    result.Add((T)outData);
                }
            }
            catch (Exception ex)
            {
                Log.Print(new LogData(LogLevel.Error, ex.Message, func?.Method.Name ?? "ActionExt"));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Возвращает первый не нулл результат
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T FirstNotNull<T>(this Func<T> func)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            foreach (var @delegate in list)
                if (@delegate.DynamicInvoke() is T result)
                    return result;

            return default;
        }

        /// <summary>
        /// Возвращает первый не нулл результат
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T2 FirstNotNull<T1, T2>(this Func<T1, T2> func, T1 arg1)
        {
            var list = func?.GetInvocationList() ?? Array.Empty<Delegate>();
            foreach (var @delegate in list)
                if (@delegate.DynamicInvoke(arg1) is T2 result)
                    return result;

            return default;
        }
    }
}