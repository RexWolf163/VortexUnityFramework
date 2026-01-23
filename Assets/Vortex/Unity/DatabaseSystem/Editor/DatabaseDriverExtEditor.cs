#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SettingsSystem.Bus;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;
using Vortex.Unity.FileSystem.Bus;
using Object = UnityEngine.Object;

namespace Vortex.Unity.DatabaseSystem
{
    public partial class DatabaseDriver
    {
        /// <summary>
        /// Кешированный список ресурсов. Очищается после заполнения индексов
        /// </summary>
        private static Object[] _resources;

        [InitializeOnLoadMethod]
        private static void EditorRegister()
        {
            if (Application.isPlaying)
                return;
            File.CreateFolders($"{Application.dataPath}/{Path}");
            Database.SetDriver(Instance);
            Instance.LoadDb();
        }

        private void LoadDb()
        {
            _recordsLink.Clear();
            _uniqRecordsLink.Clear();
            _resourcesIndex.Clear();

            var labels = Settings.Data().DatabaseLabels;
            if (labels == null || labels.Length == 0)
            {
                Debug.LogError(
                    "[DatabaseDriver] Метки (лейблы) не заданы в DatabaseSettings. Ассеты базы данных должны быть типа Addressable и помечены соответствующей меткой. Эти метки необходимо указать в DatabaseSettings.");
                return;
            }

            var ar = new List<IRecordPreset>();
            foreach (var label in labels)
            {
                var op = Addressables.LoadAssetsAsync<IRecordPreset>(label, null);
                var temp = op.WaitForCompletion();
                ar.AddRange(temp);
                Addressables.Release(op);
            }

            _resources = new Object[ar.Count];
            Array.Copy(ar.ToArray(), _resources, ar.Count);
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

            _resources = null;
            return result;
        }
    }
}
#endif