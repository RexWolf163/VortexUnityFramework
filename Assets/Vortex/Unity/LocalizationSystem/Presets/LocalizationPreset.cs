using UnityEngine;

namespace Vortex.Unity.LocalizationSystem.Presets
{
    public class LocalizationPreset : ScriptableObject
    {
        [SerializeField] private string langs;

        [SerializeField] private LocalePreset[] localeData;
    }
}