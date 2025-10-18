using UnityEngine;

namespace Vortex.Core.Logger
{
    public static partial class LogController
    {
        public static partial void Log(LogData log)
        {
            var source = log.Source as string ?? log.Source.GetType().Name;
            switch (log.Level)
            {
                case LogLevel.Common:
                    Debug.Log($"[{source}] {log.Message}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[{source}] {log.Message}");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"[{source}] {log.Message}");
                    break;
            }
        }
    }
}