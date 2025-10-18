using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Settings.Model;

namespace Vortex.Settings.Bus
{
    public partial class Settings
    {
        private const string Path = "Settings";

        private partial SettingsModel GetData()
        {
            if (_data == null)
            {
                var res = Resources.LoadAll(Path);
                if (res is { Length: > 0 })
                    _data = res[0] as SettingsModel;
            }

            if (_data == null)
                Debug.LogError("[SettingsController] Settings not found");
            return _data;
        }

        public override async Task AgentLoadAsync(CancellationToken cancellationToken) => await Task.CompletedTask;
    }
}