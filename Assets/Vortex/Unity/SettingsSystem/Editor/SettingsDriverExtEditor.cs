using UnityEditor;
using UnityEngine;
using Vortex.Unity.FileSystem.Bus;

namespace Vortex.Unity.SettingsSystem
{
    public partial class SettingsDriver
    {
        [InitializeOnLoadMethod]
        private static void EditorRegister()
        {
            File.CreateFolders($"{Application.dataPath}/Resources/{Path}");
        }
    }
}