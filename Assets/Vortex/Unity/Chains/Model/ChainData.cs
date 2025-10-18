using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Chains.Model
{
    [CreateAssetMenu(fileName = "LogicChain", menuName = "Database/LogicChain")]
    public class ChainData : DbRecord
    {
        [HideReferenceObjectPicker, SerializeReference]
        private ChainStage[] stages;

        public ChainStage[] Stages => stages.Clone() as ChainStage[];
    }
}