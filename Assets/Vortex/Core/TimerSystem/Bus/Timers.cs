using System.Collections.Generic;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;
using Vortex.Core.TimerSystem.Model;

namespace Vortex.Core.TimerSystem.Bus
{
    public class Timers : SystemController<Timers, ITimersDriver>
    {
        private static readonly SortedDictionary<string, Timer> TimersDict = new();

        protected override void OnDriverConnect()
        {
            //Ignore
        }

        /// <summary>
        /// Создать таймер
        /// </summary>
        /// <returns></returns>
        public static Timer AddNewTimer()
        {
            var timer = new Timer();
            TimersDict.AddNew(timer.Guid, timer);
            return timer;
        }

        /// <summary>
        /// Вернуть таймер по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Timer GetTimer(string guid)
        {
            if (!TimersDict.ContainsKey(guid))
            {
                Log.Print(new LogData(LogLevel.Error, $"Timer «{guid}» not found", Instance));
                return null;
            }

            return TimersDict[guid];
        }
    }
}