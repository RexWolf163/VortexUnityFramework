using UnityEditor;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Unity.DatabaseSystem.Storage;
using Vortex.Unity.FileSystem.Bus;

namespace Vortex.Unity.DatabaseSystem
{
    public partial class DatabaseDriver
    {
        [InitializeOnLoadMethod]
        private static void EditorRegister()
        {
            File.CreateFolders($"{Application.dataPath}/Resources/{Path}");
            Database.SetDriver(Instance);
            Instance.LoadDb();
        }

        public static void RefreshDb() => Instance.LoadDb();

        private void LoadDb()
        {
            _recordsLink.Clear();
            _resources = Resources.LoadAll(Path);
            foreach (var resource in _resources)
            {
                if (resource is not IRecordStorage data)
                    continue;
                AddRecord(data.GetData(), data.Guid, data.Name);
            }
        }
    }
}