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

        [MenuItem("Vortex/Localization/Load data")]
        private static async void LoadLocalizationData()
        {
            var resources = Resources.LoadAll<LocalizationPreset>(Path);
            if (resources == null || resources.Length == 0)
            {
                Debug.LogError("[Localization] Localization Preset not found]");
                return;
            }

            _resource = resources[0];
            await _resource.LoadData();
            RefreshIndex();
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
            RefreshIndex();
        }

        [MenuItem("Vortex/Localization/Update index")]
        private static void RefreshIndex()
        {
            _localeData.Clear();
            foreach (var data in _resource.localeData)
            {
                var translateData = data.Texts.First(x => x.Language == Localization.GetCurrentLanguage());
                _localeData.AddNew(data.Key, translateData.Text);
            }
        }
    }
}
#endif