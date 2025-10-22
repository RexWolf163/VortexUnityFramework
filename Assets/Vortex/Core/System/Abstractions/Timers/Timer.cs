using System;

namespace Vortex.Core.System.Abstractions.Timers
{
    public class Timer
    {
        public DateTime Start { get; protected set; }

        public DateTime End { get; protected set; }

        internal Timer(DateTime end)
        {
            Start = DateTime.UtcNow;
            End = end;
        }

        internal Timer(TimeSpan duration)
        {
            Start = DateTime.UtcNow;
            End = DateTime.UtcNow.Add(duration);
        }

        internal Timer(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Таймер завершен
        /// </summary>
        /// <returns></returns>
        public bool IsComplete() => End <= DateTime.UtcNow;

        /// <summary>
        /// Таймер завершен
        /// </summary>
        /// <returns></returns>
        public bool IsStarted() => Start <= DateTime.UtcNow;

        /// <summary>
        /// Длительность таймера
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDuration() => End - Start;

        /// <summary>
        /// Сколько времени осталось
        /// </summary>
        /// <returns>Вернет общую длительность если точка начала еще не достигнута</returns>
        public TimeSpan GetTimeRemains()
        {
            if (IsComplete())
                return TimeSpan.Zero;
            return IsStarted() ? End - DateTime.UtcNow : GetDuration();
        }

        /// <summary>
        /// Сколько времени прошло.
        /// </summary>
        /// <returns>вернет 0 если точка начала еще не достигнута</returns>
        public TimeSpan GetTimeLeft()
        {
            if (IsComplete())
                return GetDuration();
            return IsStarted() ? DateTime.UtcNow - Start : TimeSpan.Zero;
        }

        public override string ToString() => $"Timer from {Start} to {End} (duration: {GetDuration()})";
    }
}