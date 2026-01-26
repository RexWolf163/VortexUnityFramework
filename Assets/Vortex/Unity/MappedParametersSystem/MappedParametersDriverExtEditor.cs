using UnityEditor;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.MappedParametersSystem.Bus;
using Vortex.Unity.FileSystem.Bus;
using Vortex.Unity.MappedParametersSystem.Base;
using Vortex.Unity.MappedParametersSystem.Base.Preset;

#if UNITY_EDITOR
namespace Vortex.Unity.MappedParametersSystem
{
    public partial class MappedParametersDriver
    {
        private const string Path = "MapsConfig";

        [InitializeOnLoadMethod]
        private static void EditorRegister()
        {
            File.CreateFolders($"{Application.dataPath}/Resources/{Path}");
            if (!MappedParameters.SetDriver(Instance))
                return;

            Instance.LoadData();
        }

        private void LoadData()
        {
            var resources = Resources.LoadAll<ParametersMapStorage>("");
            if (resources == null || resources.Length == 0)
            {
                Debug.LogError("Localization Data asset not found");
                return;
            }

            _indexMaps.Clear();
            foreach (var map in resources)
                _indexMaps.AddNew(map.guid, GetMap(map));
        }
    }
}
#endif