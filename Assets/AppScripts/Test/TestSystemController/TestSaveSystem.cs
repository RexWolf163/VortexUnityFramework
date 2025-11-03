using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.SaveSystem.Abstraction;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Unity.SaveSystem;

namespace AppScripts.Test.TestSystemController
{
    public class TestSaveSystem : MonoBehaviour
    {
        [ShowInInspector, ListDrawerSettings(HideAddButton = true)]
        private List<SaveSlot> _slots = new();

        [HideReferenceObjectPicker]
        private class SaveSlot
        {
            [HorizontalGroup("h1"), VerticalGroup("h1/v1"), ShowInInspector, HideLabel, DisplayAsString]
            private string _name;

            [HorizontalGroup("h1"), VerticalGroup("h1/v1"), ShowInInspector, HideLabel, DisplayAsString]
            private string _timestamp;

            private string _guid;

            public SaveSlot(string guid, SaveSummary summary)
            {
                _name = summary.Name;
                _timestamp = summary.Date.ToString("f");
                _guid = guid;
            }

            [HorizontalGroup("h1", 60f), HideIf("Test")]
            [Button]
            public void Load() => SaveController.Load(_guid);

            private bool Test() => !Application.isPlaying;
        }

        [HorizontalGroup("h1")]
        [Button, HideIf("Test")]
        private void Save() => SaveController.Save("Save");

        [HorizontalGroup("h1")]
        [Button]
        private void GetIndex()
        {
            var data = SaveController.GetIndex();
            _slots.Clear();
            foreach (var save in data)
            {
                _slots.Add(new SaveSlot(save.Key, save.Value));
                Debug.Log($"<b>{save.Value.Name}: {save.Value.Date:g} #{save.Key}</b>");
            }
        }

        private bool Test() => !Application.isPlaying;
    }
}