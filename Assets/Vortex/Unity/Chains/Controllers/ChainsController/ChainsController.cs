using System.Collections.Generic;
using UnityEngine;
using Vortex.Chains.Model;
using Vortex.Core.App;

namespace Vortex.Chains.Controllers
{
    public partial class ChainsController : IChainsController
    {
        private Dictionary<string, ChainData> chains;

        private static ChainsController _instance;

        private static ChainsController Instance => _instance ?? AppController.GetSystem<ChainsController>();

        /// <summary>
        /// Возвращает цепочку настроенную в БД по ее guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IChain GetChain(string guid)
        {
            if (!chains.TryGetValue(guid, out var chainData))
            {
                Debug.LogError($"[ChainsController] No chain found for guid: {guid}");
                return null;
            }

            var chain = new Chain(chainData);
            return chain;
        }

        public static IChain GetChainRecord(string guid) => Instance.GetChain(guid);
    }
}