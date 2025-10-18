using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Chains.Controllers;
using Vortex.Chains.Model;

namespace Vortex.Chains
{
    public class ChainHandler : MonoBehaviour
    {
        [SerializeField, ValueDropdown("@ChainsController.GetChains()"), HideIf("@data!=null")]
        private string chainData;

        private IChain _chain;

        [SerializeField, HideIf("@(chainData!=\"\")")]
        private ChainData data;

        [SerializeField] private bool selfDestroyOnComplete = false;


        public void Run()
        {
            _chain = data == null ? ChainsController.GetChainRecord(chainData) : new Chain(data);
            _chain.Init();
            if (selfDestroyOnComplete)
                _chain.OnComplete += SelfDestroy;
        }

        private void SelfDestroy()
        {
            _chain.OnComplete -= SelfDestroy;
            Destroy(gameObject);
        }

        private void OnDestroy() => _chain.Destroy();
    }
}