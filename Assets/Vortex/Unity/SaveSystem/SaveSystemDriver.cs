using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SaveSystem;
using Vortex.Core.SaveSystem.Abstraction;
using Vortex.Unity.SaveSystem.Presets;

namespace Vortex.Unity.SaveSystem
{
    public partial class SaveSystemDriver : IDriver
    {
        private const string SaveKey = "SavesData";
        private const string SavePrefix = "Save-";

        private static HashSet<string> _saves = new();

        private static Dictionary<string, string> _saveDataIndex;

        public event Action OnInit;

        public void Init()
        {
            _saves.Clear();
            var saves = PlayerPrefs.GetString(SaveKey);
            var ar = saves.Split(';');
            if (ar.Length > 0)
                _saves.AddRange(ar);
            OnInit?.Invoke();
        }

        public void Destroy()
        {
        }

        public void Save(string guid)
        {
            var count = _saveDataIndex.Count;
            var save = new SavePreset
            {
                data = new List<SaveData>()
            };
            var list = _saveDataIndex.Keys.ToArray();
            for (var i = 0; i < count; i++)
            {
                var data = _saveDataIndex[list[i]];
                save.data.Add(new SaveData { Id = list[i], Data = data });
            }

            var xmls = new XmlSerializer(typeof(SavePreset));
            var sw = new StringWriter();
            xmls.Serialize(sw, save);
            var saveData = sw.ToString();

            PlayerPrefs.SetString(GetSaveName(guid), saveData.Compress(guid));

            _saves.Add(guid);
            PlayerPrefs.SetString(SaveKey, string.Join(";", _saves));
        }

        public void Load(string guid)
        {
            _saveDataIndex.Clear();
            if (!PlayerPrefs.HasKey(GetSaveName(guid)))
            {
                Debug.LogError($"[SaveSystemDriver] save data #{guid} not found.");
                return;
            }

            var saveData = PlayerPrefs.GetString(GetSaveName(guid));
            saveData = saveData.Decompress(guid);

            var xmls = new XmlSerializer(typeof(SavePreset));
            var save = xmls.Deserialize(new StringReader(saveData)) as SavePreset;
            if (save == null)
            {
                Debug.LogError($"[SaveSystemDriver] Error while loading save data from {guid}.]");
                return;
            }

            foreach (var item in save.data)
                _saveDataIndex.AddNew(item.Id, item.Data);
        }

        public void SetIndexLink(Dictionary<string, string> index) => _saveDataIndex = index;

        /// <summary>
        /// Формирование названия сейва по guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private static string GetSaveName(string guid) => $"{SavePrefix}{guid}";

        /// <summary>
        /// Возвращает все существующие сейвы
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetIndex() => _saves;
    }
}