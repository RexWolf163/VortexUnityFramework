using System;

namespace Vortex.Core.Logger
{
    public struct LogData
    {
        public LogLevel Level { get; }
        public string Message { get; }
        public Object Source { get; }

        public LogData(LogLevel level, string message, Object source)
        {
            Level = level;
            Message = message;
            Source = source;
        }
    }
}