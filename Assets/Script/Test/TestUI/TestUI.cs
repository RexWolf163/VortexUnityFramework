//using Vortex.UI;

using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;

namespace App.UI
{
    public class TestUI : MonoBehaviour // : UIBase
    {
        [SerializeField, ValueDropdown("@DatabaseHandler.GetRecords()")]
        private string db;

        private TestItem item;

        [Button]
        private void Test()
        {
            item = Database.GetRecord<TestItem>(db);
        }

        private void Awake()
        {
            item = Database.GetRecord<TestItem>(db);
        }
    }
}