using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.SaveSystem.Bus;

namespace AppScripts.Test.TestSystemController
{
    public class TestSaveSystem : MonoBehaviour
    {
        [Button]
        private void Save()
        {
            SaveController.Save();
        }

        [Button]
        private void GetIndex()
        {
            var data = SaveController.GetIndex();
            foreach (var save in data)
            {
                Debug.Log($"<b>{save}</b>");
            }
        }

        [Button]
        private void Load(string guid) => SaveController.Load(guid);
    }
}