using UnityEngine;
using Vortex.Unity.DatabaseSystem.Storage;

[CreateAssetMenu(fileName = "TestItem", menuName = "Database/Test Item")]
public class TestItemStorage : RecordStorage<TestItem>
{
    [SerializeField] //, ValueDropdown("@ChainsController.GetChains()")]
    private string chain;

    public string Chain => chain;
}