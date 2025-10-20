using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SaveSystem;
using Vortex.Unity.SaveSystem.Storage;

namespace Vortex.Unity.SaveSystem
{
    public partial class SaveSystemDriver : IDriver
    {
        private const string SaveKey = "SavesData";
        private const string SavePrefix = "Save-";

        private static HashSet<string> _saves = new();

        private static Dictionary<string, string> _saveDataIndex;

        public void Init()
        {
            _saves.Clear();
            var saves = PlayerPrefs.GetString(SaveKey);
            var ar = saves.Split(';');
            if (ar.Length > 0)
                _saves.AddRange(ar);
        }

        public void Destroy()
        {
        }

        public async Task Save(string guid, CancellationToken cancellationToken)
        {
            var count = _saveDataIndex.Count;
            var save = new SaveStorage[count];
            var list = _saveDataIndex.Keys.ToArray();
            for (var i = 0; i < count; i++)
            {
                var data = _saveDataIndex[list[i]];
                save[i] = new SaveStorage()
                {
                    Id = list[i],
                    Data = data
                };
            }

            var xmls = new XmlSerializer(typeof(SaveStorage[]));
            var sw = new StringWriter();
            xmls.Serialize(sw, save);
            var saveData = sw.ToString();

            PlayerPrefs.SetString(GetSaveName(guid), saveData);

            await Task.CompletedTask;
        }

        public async Task Load(string guid, CancellationToken cancellationToken)
        {
            _saveDataIndex.Clear();
            var saveData = PlayerPrefs.GetString(GetSaveName(guid));
            saveData = saveData.Decompress(guid);

            var xmls = new XmlSerializer(typeof(SaveStorage[]));
            var save = xmls.Deserialize(new StringReader(saveData)) as SaveStorage[];
            if (save == null)
            {
                Debug.LogError($"[SaveSystemDriver] Error while loading save data from {guid}.]");
                await Task.CompletedTask;
                return;
            }

            foreach (var item in save)
                _saveDataIndex.AddNew(item.Id, item.Data);

            await Task.CompletedTask;
        }

        public void SetIndexLink(Dictionary<string, string> index) => _saveDataIndex = index;

        /// <summary>
        /// Формирование названия сейва по guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private static string GetSaveName(string guid) => $"{SavePrefix}{guid}";
    }
}