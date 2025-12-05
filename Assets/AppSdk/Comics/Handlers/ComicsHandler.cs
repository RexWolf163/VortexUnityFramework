using AppSdk.Comics.Database;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Attributes;

namespace AppSdk.Comics.Handlers
{
    public class ComicsHandler : MonoBehaviour
    {
        [SerializeField, DbRecord(typeof(ComicsData))]
        private string comicsId;

        [Button]
        private void CallComics() => ComicsController.StartComics(comicsId);
    }
}