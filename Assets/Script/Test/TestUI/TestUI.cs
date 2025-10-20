//using Vortex.UI;

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.SaveSystem.Abstraction;
using Vortex.Core.SaveSystem.Bus;

namespace App.UI
{
    public class TestUI : MonoBehaviour // : UIBase
    {
        [SerializeField, ValueDropdown("@DatabaseHandler.GetRecords()")]
        private string db;

        private TestItem item;

        /// <summary>
        /// токен-ресурс прерывания
        /// </summary>
        private static readonly CancellationTokenSource CancelTokenSource = new();

        /// <summary>
        /// Токен прерывания
        /// </summary>
        private static CancellationToken Token => CancelTokenSource.Token;

        [Button]
        private void Test()
        {
            item = Database.GetRecord<TestItem>(db);
        }

        private void Awake()
        {
            item = Database.GetRecord<TestItem>(db);
        }

        [Button]
        private void Save()
        {
            SaveController.OnSave -= OnSave;
            SaveController.OnSave += OnSave;
            SaveController.Save("4f748eb667f1c922bb6441ad2c62abe7948bdc8d74a04c86cb8acef4828190ed");
            SaveController.OnSave -= OnSave;
        }

        [Button]
        private void GetIndex()
        {
            var list = SaveController.GetIndex();
        }

        private SaveData OnSave()
        {
            return new SaveData()
            {
                Id = GetType().Name,
                Data = JsonUtility.ToJson(item),
            };
        }

        [Button]
        private void Load()
        {
            SaveController.OnLoad -= OnLoad;
            SaveController.OnLoad += OnLoad;
            SaveController.Load("4f748eb667f1c922bb6441ad2c62abe7948bdc8d74a04c86cb8acef4828190ed");
            SaveController.OnLoad -= OnLoad;
        }

        private void OnLoad()
        {
            var data = SaveController.GetData(GetType().Name);
        }
    }
}