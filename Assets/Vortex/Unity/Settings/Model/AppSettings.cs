using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.Settings;
using Vortex.Unity.UI.Components;

[CreateAssetMenu(fileName = "AppSettings", menuName = "AppSettings")]
public partial class AppSettings : ScriptableObject, IAppSettings
{
    [ValueDropdown("Scenes")] [SerializeField]
    private string startScene;

    [SerializeField] private bool debugMode = true;

    public string StartScene => startScene;

    public bool DebugMode => debugMode;

#if UNITY_EDITOR
    private List<string> Scenes() => SceneController.GetScenes();
#endif
}