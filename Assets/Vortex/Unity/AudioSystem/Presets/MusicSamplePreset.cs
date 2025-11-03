using UnityEngine;
using Vortex.Core.AudioSystem.Model;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;

namespace Vortex.Unity.AudioSystem.Presets
{
    [CreateAssetMenu(fileName = "MusicSample", menuName = "Database/MusicSample")]
    public class MusicSamplePreset : RecordPreset<MusicSample>
    {
        [SerializeField] private AudioClip audioClip;

        public object Sample => audioClip;

#if UNITY_EDITOR
        private void OnValidate() => type = RecordTypes.Singleton;
#endif
    }
}