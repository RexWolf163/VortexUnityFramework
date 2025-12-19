using AppScripts.Narrative.Model;
using Articy.Unity;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;

namespace AppScripts.Narrative.DatabaseSystem
{
    [CreateAssetMenu(fileName = "Character", menuName = "Narrative/Character Preset")]
    public class NrCharacterPreset : RecordPreset<NrCharacterModel>
    {
        [SerializeReference, HideReferenceObjectPicker]
        private ArticyRef articyRef = new();

        public ArticyRef Ref => articyRef;

#if UNITY_EDITOR
        private void OnValidate()
        {
            type = RecordTypes.Singleton;
        }
#endif
    }
}