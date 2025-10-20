using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.LoggerSystem.Bus
{
    public class Log : SystemController<Log, IDriver>
    {
        public static void Print(LogData log)
        {
            Driver.Print(log);
        }

        protected override void OnDriverConnect()
        {
            //Ignore
        }
    }
}