using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using System;
using UnityEditor;
using Vortex.Extensions;
#endif

namespace Vortex.Database.Model.Record
{
    public abstract partial class Record : ScriptableObject
    {
        /// <summary>
        /// GUID
        /// </summary>
        [SerializeField, DisplayAsString] private string guid;

        /// <summary>
        /// Наименование записи
        /// </summary>
        [SerializeField, OnValueChanged("OnNameChanged")]
        private string nameRecord;

        /// <summary>
        /// Описание записи
        /// </summary>
        [SerializeField] private string description;

        /// <summary>
        /// Иконка для записи
        /// </summary>
        [PreviewField] private Sprite icon = null;

        /// <summary>
        /// Получить GUID
        /// </summary>
        /// <returns></returns>
        public partial string GetGuid() => guid;

        /// <summary>
        /// Получить наименование
        /// </summary>
        /// <returns></returns>
        public partial string GetName() => nameRecord;

        /// <summary>
        /// Получить описание
        /// </summary>
        /// <returns></returns>
        public partial string GetDescription() => description;

        /// <summary>
        /// Получить иконку
        /// </summary>
        /// <returns></returns>
        public Sprite GetIcon() => icon;

#if UNITY_EDITOR
        [Button]
        public void ResetGuid() => MakeGuid();

        private void OnNameChanged()
        {
            var number = 0;
            while (true)
            {
                var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                var result = AssetDatabase.RenameAsset(assetPath, $"{nameRecord}{(number > 0 ? $" ({number})" : "")}");
                if (result != "")
                {
                    number++;
                    continue;
                }

                AssetDatabase.SaveAssets();
                break;
            }

            if (number > 0)
                Debug.LogError($"[DbRecord] Name {nameRecord} for records already exists!");
        }

        private partial void MakeGuid()
        {
            var temp = DateTime.UtcNow.ToFileTimeUtc().ToString() + GetInstanceID();
            guid = Crypto.GetHashSha256(temp);
        }
#endif
    }
}