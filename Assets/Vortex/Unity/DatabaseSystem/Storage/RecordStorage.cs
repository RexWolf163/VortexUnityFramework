using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions.SystemControllers;
using Vortex.Unity.Extensions.Abstractions;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Vortex.Unity.DatabaseSystem.Storage
{
    public abstract class RecordStorage<T> : SoData, IRecordStorage where T : Record, new()
    {
        private const string DefaultName = "DBItem";

        [SerializeField, DisplayAsString] private string guid;

        [SerializeField, OnValueChanged("OnNameChanged"), LabelText("Name")]
        private string nameRecord;

        [SerializeField] private string description;
        [PreviewField, SerializeField] private Sprite icon;

        /// <summary>
        /// Глобально уникальный идентификатор 
        /// </summary>
        public string Guid => guid;

        /// <summary>
        /// Наименование элемента БД
        /// </summary>
        public string Name => nameRecord;

        /// <summary>
        /// Описание элемента БД
        /// </summary>
        public string Description => description;

        /// <summary>
        /// Иконка элемента
        /// </summary>
        public Sprite Icon => icon;

        /// <summary>
        /// Возвращает модель данных заполненную из хранилища
        /// </summary>
        /// <returns></returns>
        public Record GetData()
        {
            var record = new T();
            record.CopyFrom(this);
            return record;
        }

#if UNITY_EDITOR
        public RecordStorage() => guid = Crypto.GetNewGuid();

        [Button]
        public void ResetGuid() => guid = Crypto.GetNewGuid();

        private void OnNameChanged()
        {
            return;
            /*
            var number = 0;
            var name = nameRecord;
            if (name == string.Empty) name = DefaultName;

            while (true)
            {
                var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                var result =
                    AssetDatabase.RenameAsset(assetPath, $"{name}{(number > 0 ? $" ({number})" : "")}");
                if (result != "")
                {
                    number++;
                    continue;
                }

                AssetDatabase.SaveAssets();
                break;
            }

            if (number > 0 && nameRecord != string.Empty)
                Debug.LogError($"[DbRecord] Name {name} for records already exists!");
        */
        }
#endif
    }
}