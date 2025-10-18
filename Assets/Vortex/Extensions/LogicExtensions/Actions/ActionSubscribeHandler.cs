using System;
using System.Linq;

namespace Vortex.Extensions
{
    /// <summary>
    /// Расширение класса Action для очобых вариантов подписки
    /// (Синтаксический сахар)
    /// </summary>
    public static class ActionSubscribeHandler
    {
        public static void AddOnce(Action eventAction, Action del)
        {
            var list = eventAction.GetInvocationList();
            if (list.Contains(del))
                return;
            eventAction += del;
        }

        public static void AddOnce<T>(Action<T> eventAction, Action<T> del)
        {
            var list = eventAction.GetInvocationList();
            if (list.Contains(del))
                return;
            eventAction += del;
        }
    }
}