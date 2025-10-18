using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Database;

namespace Script.Test
{
    [CreateAssetMenu(fileName = "TestItem", menuName = "Database/Test Item")]
    public class DbItem : DbRecord
    {
        [SerializeField, ValueDropdown("@ChainsController.GetChains()")]
        private string chain;
    }
}