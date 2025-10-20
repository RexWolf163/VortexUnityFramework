using UnityEngine;
using Vortex.Core.SettingsSystem;
using Vortex.Core.SettingsSystem.Model;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.Abstractions.SystemControllers;
using Vortex.Unity.FileSystem.Bus;
using Vortex.Unity.SettingsSystem.Storage;

namespace Vortex.Unity.SettingsSystem
{
    public partial class SettingsDriver : Singleton<SettingsDriver>, IDriver
    {
        private const string Path = "Settings";

        private SettingsModel _model;

        private SettingsModel Model
        {
            get
            {
                if (_model != null)
                    return _model;
                _model = new SettingsModel();
                LoadData();

                return _model;
            }
        }

        private void CheckPath() => File.CreateFolders($"{Application.dataPath}/Resources/{Path}");

        private bool LoadData()
        {
            CheckPath();
            var dataSets = Resources.LoadAll<SettingsStorage>(Path);
            foreach (var data in dataSets)
            {
                var result = Model.CopyFrom(data);
                if (result)
                    continue;
                Debug.LogError($"[SettingsDriver] Failed to load settings data from {Path}");
                return false;
            }

            return true;
        }

        public void Init() => LoadData();

        public void Destroy()
        {
            //Ignore
        }

        public SettingsModel GetData() => Model;
    }
}