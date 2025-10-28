#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Unity.FileSystem.Bus;
using Vortex.Unity.LocalizationSystem.Presets;

namespace Vortex.Unity.LocalizationSystem
{
    public partial class LocalizationDriver
    {
        [InitializeOnLoadMethod]
        private static void EditorRegister()
        {
            File.CreateFolders($"{Application.dataPath}/Resources/{Path}");
            var resources = Resources.LoadAll<LocalizationPreset>(Path);
            if (resources == null || resources.Length == 0)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<LocalizationPreset>(),
                    $"Assets/Resources/{Path}/LocalizationData.asset");
                Debug.Log("Create new settings preset LocalizationData");
            }

            Localization.SetDriver(Instance);
            Instance.LoadData();
        }

        [MenuItem("Vortex/Update Localization data")]
        private static void LoadLocalizationData()
        {
            var resources = Resources.LoadAll<LocalizationPreset>(Path);
            if (resources == null || resources.Length == 0)
            {
                Debug.LogError("[Localization] Localization Preset not found]");
                return;
            }

            _resource = resources[0];
            _resource.LoadData();
        }

        private void LoadData()
        {
            var resources = Resources.LoadAll<LocalizationPreset>(Path);
            if (resources == null || resources.Length == 0)
            {
                Debug.LogError("Localization Data asset not found");
                return;
            }

            _resource = resources[0];
            foreach (var data in _resource.localeData)
            {
                var translateData = data.Texts.First(x => x.Language == Localization.GetCurrentLanguage().ToString());
                _localeData.AddNew(data.Key, translateData.Text);
            }
        }
    }
}
#endif