using UnityEngine;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;

namespace AppSdk.Comics.Database
{
    [CreateAssetMenu(fileName = "Comics", menuName = "Database/Comics Preset")]
    public class ComicsPreset : RecordPreset<ComicsData>
    {
        [SerializeField] private ComicsStage[] stages = new ComicsStage[0];

        /// <summary>
        /// Этапы комикса
        /// </summary>
        public ComicsStage[] ComicsStages => stages;

#if UNITY_EDITOR
        private void OnValidate()
        {
            type = RecordTypes.Singleton;
        }
#endif
    }
}