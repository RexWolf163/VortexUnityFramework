using UnityEngine;
using Vortex.Unity.DatabaseSystem.Storage;

namespace AppScripts.Test
{
    [CreateAssetMenu(fileName = "TestItem", menuName = "Database/Test Item")]
    public class TestItemStorage : RecordStorage<TestItem>
    {
        [SerializeField] private string chain;

        public string Chain => chain;
    }
}