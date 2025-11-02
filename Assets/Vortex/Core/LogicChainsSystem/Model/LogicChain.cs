using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.Extensions.LogicExtensions;
using NotImplementedException = System.NotImplementedException;

namespace Vortex.Core.LogicChainsSystem.Model
{
    public class LogicChain : Record
    {
        /// <summary>
        /// Этапы цепочки
        /// </summary>
        public SortedDictionary<string, ChainStep> ChainSteps { get; protected set; }

        /// <summary>
        /// GUID начального этапа
        /// </summary>
        public string StartStep { get; protected set; }

        /// <summary>
        /// GUID текущего этапа
        /// </summary>
        public string CurrentStep { get; protected internal set; }

        public override string GetDataForSave() => CurrentStep;

        public override void LoadFromSaveData(string data) => CurrentStep = data;
    }
}