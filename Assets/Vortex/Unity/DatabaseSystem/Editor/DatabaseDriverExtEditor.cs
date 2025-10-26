#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Unity.DatabaseSystem.Presets;
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

        private void LoadDb()
        {
            _recordsLink.Clear();
            _resources = Resources.LoadAll(Path);
            foreach (var resource in _resources)
            {
                if (resource is not IRecordPreset data)
                    continue;
                AddRecord(data.GetData(), data.Guid, data.Name);
            }
        }

        internal ValueDropdownList<string> GetDropdownList()
        {
            var result = new ValueDropdownList<string>();
            Instance.LoadDb();

            foreach (var record in _resources)
            {
                if (record is not IRecordPreset item)
                    continue;
                var path = AssetDatabase.GetAssetPath(record.GetInstanceID());
                var tempAr = path.Split(Path + "/");
                if (tempAr.Length == 0)
                    continue;
                path = tempAr[1];
                result.Add(path, item.Guid);
            }

            return result;
        }
    }
}
#endif