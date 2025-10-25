using System.Threading;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.UIProviderSystem.Model;

namespace AppScripts.Test.TestUI
{
    public class TestUI : UserInterface
    {
        [SerializeField, DbRecord(typeof(TestItem))]
        private string dbRecord;

        [SerializeField, DbRecord(typeof(TestItem))]
        private string dbRecord2;

        private TestItem item;
        private TestItem item2;

        /// <summary>
        /// токен-ресурс прерывания
        /// </summary>
        private static readonly CancellationTokenSource CancelTokenSource = new();

        /// <summary>
        /// Токен прерывания
        /// </summary>
        private static CancellationToken Token => CancelTokenSource.Token;

        private void Awake()
        {
            Database.OnInit += Init;
        }

        private void OnDestroy()
        {
            Database.OnInit -= Init;
        }

        private void Init()
        {
            item = Database.GetRecord<TestItem>(dbRecord);
            item2 = Database.GetRecord<TestItem>(dbRecord2);
        }
    }
}