using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;
using NotImplementedException = System.NotImplementedException;

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

        protected override void OnDriverDisonnect()
        {
            //Ignore
        }
    }
}