using Articy.Unity;
using UnityEngine;
using Vortex.Unity.SettingsSystem.Presets;

namespace AppScripts.Narrative.DatabaseSystem
{
    public class NrSettings : SettingsPreset
    {
        [SerializeField] private ArticyRef narrativeCodex;

        public ArticyRef NarrativeCodex => narrativeCodex;

        [SerializeField] private ArticyRef[] narrativeParts = new ArticyRef[0];

        public ArticyRef[] NarrativeParts => narrativeParts;
    }
}