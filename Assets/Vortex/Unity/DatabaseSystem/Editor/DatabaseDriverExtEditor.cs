#if UNITY_EDITOR
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Unity.DatabaseSystem.Enums;
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
            _uniqRecordsLink.Clear();
            _resourcesIndex.Clear();
            _resources = Resources.LoadAll(Path);
            foreach (var resource in _resources)
            {
                if (resource is not IRecordPreset data)
                    continue;
                AddRecord(data.GetData(), data);
                _resourcesIndex.AddNew(data.GuidPreset, data);
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
                result.Add(path, item.GuidPreset);
            }

            return result;
        }

        public ValueDropdownList<string> GetDropdownList(Type recordClass, RecordTypes? recordType = null)
        {
            var result = new ValueDropdownList<string>();
            Instance.LoadDb();

            foreach (var resource in _resources)
            {
                if (resource is not IRecordPreset item)
                    continue;
                var record = item.GetData();
                if (!recordClass.IsInterface
                    && record.GetType() != recordClass
                    && !record.GetType().IsSubclassOf(recordClass))
                    continue;
                if (recordClass.IsInterface
                    && !record.GetType().GetInterfaces().Contains(recordClass))
                    continue;
                if (recordType != null && recordType != item.RecordType)
                    continue;
                var path = AssetDatabase.GetAssetPath(resource.GetInstanceID());
                var tempAr = path.Split(Path + "/");
                if (tempAr.Length == 0)
                    continue;
                path = tempAr[1];
                result.Add(path, item.GuidPreset);
            }

            return result;
        }
    }
}
#endif