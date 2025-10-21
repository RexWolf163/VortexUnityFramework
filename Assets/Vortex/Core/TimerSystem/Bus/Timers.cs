using System;
using System.Collections.Generic;
using System.Linq;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.System.Abstractions;
using Vortex.Core.TimerSystem.Model;

namespace Vortex.Core.TimerSystem.Bus
{
    public partial class Timers : SystemController<Timers, ITimersDriver>
    {
        private static readonly SortedDictionary<string, TimerInstance> Index = new();

        protected override void OnDriverConnect() => SaveController.Register(Instance);

        protected override void OnDriverDisonnect() => SaveController.UnRegister(Instance);

        /// <summary>
        /// Создать таймер
        /// </summary>
        /// <param name="end">Время завершения таймера</param>
        /// <returns></returns>
        public static TimerInstance AddNewTimer(DateTime end)
        {
            var timer = new TimerInstance(end);
            Index.AddNew(timer.Guid, timer);
            return timer;
        }

        /// <summary>
        /// Создать таймер
        /// </summary>
        /// <param name="duration">Длительность таймера</param>
        /// <returns></returns>
        public static TimerInstance AddNewTimer(TimeSpan duration)
        {
            var timer = new TimerInstance(DateTime.UtcNow.Add(duration));
            Index.AddNew(timer.Guid, timer);
            return timer;
        }

        /// <summary>
        /// Вернуть таймер по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static TimerInstance GetTimer(string guid)
        {
            if (Index.TryGetValue(guid, out var timer))
                return timer;
            Log.Print(new LogData(LogLevel.Error, $"Timer «{guid}» not found", Instance));
            return null;
        }

        /// <summary>
        /// Все таймеры в системе
        /// </summary>
        /// <returns></returns>
        public static List<TimerInstance> GetAllTimers() => Index.Values.ToList();
    }
}