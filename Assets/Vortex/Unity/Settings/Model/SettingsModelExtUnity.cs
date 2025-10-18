using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Settings.Model
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "AppSettings")]
    public partial class SettingsModel : ScriptableObject
    {
        [ValueDropdown("Scenes")] [SerializeField]
        private string startScene;

        [SerializeField] private bool debugMode = true;

        public string StartScene => startScene;

#if UNITY_EDITOR
        private List<string> Scenes() => null; //SceneController.GetScenes();
#endif
    }
}