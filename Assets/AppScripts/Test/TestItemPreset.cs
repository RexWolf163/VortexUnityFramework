using UnityEngine;
using Vortex.Unity.DatabaseSystem.Presets;

namespace AppScripts.Test
{
    [CreateAssetMenu(fileName = "TestItem", menuName = "Database/Test Item")]
    public class TestItemPreset : RecordPreset<TestItem>
    {
        [SerializeField] private string chain;

        public string Chain => chain;
    }
}