using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AppSettings", menuName = "AppSettings")]
public partial class SettingsModel : ScriptableObject
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