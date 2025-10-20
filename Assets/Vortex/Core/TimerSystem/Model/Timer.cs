using Vortex.Core.Extensions.LogicExtensions;

namespace Vortex.Core.TimerSystem.Model
{
    public class Timer
    {
        public string Guid { get; }

        public Timer()
        {
            Guid = Crypto.GetNewGuid();
        }
    }
}