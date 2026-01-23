using System.Text;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Vortex.Core.System.Abstractions;
using Vortex.Unity.CoreAssetsSystem;
using UnityEditor;
#endif

namespace Vortex.Unity.DriverManagerSystem.Base
{
    /// <summary>
    /// Настройки используемых системами драйверов.
    /// Предназначен для однозначного указания какой именно драйвер будет использоваться системой
    /// (DI паттерн на минималках)
    /// </summary>
    public class DriverConfig : ScriptableObject, ICoreAsset
    {
        [SerializeReference, HideReferenceObjectPicker, ListDrawerSettings(HideAddButton = true, IsReadOnly = true)]
        private DriverRecord[] drivers;

        [InfoBox("Искать драйверы только в ru.vortex* пакетах")] [SerializeField]
        private bool onlyInVortexSearch = true;

        /// <summary>
        /// Возвращает назначенный указанной системе драйвер
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public string GetDriverForSystem(string systemName)
        {
            var rec = drivers.FirstOrDefault(r => r.SystemType == systemName);
            if (rec == null)
                return null;
            return rec.DriverType;
        }

#if UNITY_EDITOR
        [Button("Reload")]
        private void ReloadList()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (onlyInVortexSearch)
                assemblies = assemblies.Where(a => a.FullName.StartsWith("ru.vortex")).ToArray();

            var result = new List<DriverRecord>();
            var index = drivers?.Select(d => d.SystemType).Where(s => !s.IsNullOrWhitespace())
                .ToArray() ?? Array.Empty<string>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface
                    && typeof(ISystemController).IsAssignableFrom(t));

                foreach (var system in types)
                {
                    var typeName = system.AssemblyQualifiedName;
                    if (drivers != null && index.Contains(typeName))
                    {
                        result.Add(drivers.FirstOrDefault(d => d.SystemType == typeName));
                        continue;
                    }

                    var rec = new DriverRecord(system.AssemblyQualifiedName);
                    result.Add(rec);
                }
            }

            drivers = result.ToArray();

            SaveConfig();
        }

        private const string CfgFileName = "DriversGenericList.cs";

        private void SaveConfig()
        {
            var assetsPath = Application.dataPath;
            var allFiles = Directory.GetFiles(assetsPath, CfgFileName, SearchOption.AllDirectories);

            if (allFiles.Length <= 0)
            {
                Debug.Log($"[DriversListFileGenerator] Не найден файл DriversGenericList.cs.");
                return;
            }

            var targetPath = allFiles[0];
            File.WriteAllText(targetPath, GetFileContent(), Encoding.UTF8);

            if (targetPath.StartsWith(assetsPath))
            {
                string assetPath = "Assets" + targetPath.Substring(assetsPath.Length).Replace("\\", "/");
                AssetDatabase.ImportAsset(assetPath);
            }

            Debug.Log($"[DriversListFileGenerator] Конфигурация записана");
        }

        /// <summary>
        /// Кодогенерация файла конфигурации
        /// </summary>
        /// <returns></returns>
        private string GetFileContent()
        {
            var sb = new StringBuilder();
            sb.Append(
                "using System.Collections.Generic;\n\nnamespace Vortex.Core.System\n{\n    public static class DriversGenericList\n    {\n        public static Dictionary<string, string> WhiteList { get; } = new()\n        {");

            foreach (var driver in drivers)
                sb.Append(
                    $"\n            {{\n                \"{driver.SystemType}\",\n                \"{driver.DriverType}\"\n            }},");
            sb.Append("\n        };\n    }\n}");

            return sb.ToString();
        }
#endif
    }
}