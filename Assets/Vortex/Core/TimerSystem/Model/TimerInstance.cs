using System;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions.SystemControllers;

namespace Vortex.Core.TimerSystem.Model
{
    public class TimerInstance : SystemModel
    {
        public string Guid { get; protected set; }

        public DateTime Start { get; protected set; }

        public DateTime End { get; protected set; }

        internal TimerInstance(DateTime end)
        {
            Guid = Crypto.GetNewGuid();
            Start = DateTime.UtcNow;
            End = end;
        }

        internal TimerInstance(string guid, DateTime start, DateTime end)
        {
            Guid = guid;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Таймер завершен
        /// </summary>
        /// <returns></returns>
        public bool IsComplete() => End <= DateTime.UtcNow;

        /// <summary>
        /// Длительность таймера
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDuration() => End - Start;

        /// <summary>
        /// Времени осталось
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeRemains()
        {
            if (IsComplete())
                return TimeSpan.Zero;
            return End - DateTime.UtcNow;
        }

        /// <summary>
        /// Времени прошло
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeLeft()
        {
            if (IsComplete())
                return GetDuration();
            return DateTime.UtcNow - Start;
        }

        public override string ToString() => $"timer from {Start} to {End} #{Guid}";
    }
}