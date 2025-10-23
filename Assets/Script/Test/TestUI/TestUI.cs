using System.Threading;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Unity.DatabaseSystem.Editor.Attributes;
using Vortex.Unity.UIProviderSystem.Model;

namespace App.UI
{
    public class TestUI : UserInterface
    {
        [SerializeField, DbRecord]
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
    }
}