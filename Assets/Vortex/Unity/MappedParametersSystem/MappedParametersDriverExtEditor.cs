using UnityEditor;
using UnityEngine;
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
            var resources = Resources.LoadAll<ParametersMap>("");
            if (resources == null || resources.Length == 0)
            {
                Debug.LogError("Localization Data asset not found");
                return;
            }

            _indexMaps.Clear();
            foreach (var map in resources)
            {
                var name = map.GetType().AssemblyQualifiedName;
                if (name != null)
                    _indexMaps.Add(name, new MappedParametersGroup(map));
            }
        }
    }
}
#endif