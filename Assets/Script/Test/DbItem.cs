using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Database.Model.Record;

namespace Script.Test
{
    [CreateAssetMenu(fileName = "TestItem", menuName = "Database/Test Item")]
    public class DbItem : Record
    {
        [SerializeField, ValueDropdown("@ChainsController.GetChains()")]
        private string chain;
    }
}