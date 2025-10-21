using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SaveSystem;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.System.Loadable;
using Vortex.Core.TimerSystem.Model;

namespace Vortex.Core.TimerSystem.Bus
{
    public partial class Timers : ISaveable
    {
        public string GetSaveId() => nameof(Timers);

        public string GetSaveData()
        {
            var list = Index.Values.ToList();
            var data = new List<string>();
            foreach (var timer in list)
                data.Add($"{timer.Guid}\t{timer.Start.ToUnixTime()}\t{timer.End.ToUnixTime()}");

            return string.Join("\n", data);
        }

        public void OnLoad()
        {
            Index.Clear();
            var data = SaveController.GetData(GetSaveId());
            if (string.IsNullOrEmpty(data))
                return;
            var list = data.Split("\n");
            foreach (var timerData in list)
            {
                var ar = timerData.Split("\t");
                var guid = ar[0];
                long.TryParse(ar[1], out var start);
                long.TryParse(ar[2], out var end);
                var timer = new TimerInstance(guid, new DateTime().FromUnixTime(start), new DateTime().FromUnixTime(end));
                Index.Add(timer.Guid, timer);
            }
        }

        public LoadingData GetProcessInfo() => null;

        public async Task RunAsync(CancellationToken cancellationToken) => await Task.CompletedTask;

        public Type[] WaitingFor() => null;
    }
}