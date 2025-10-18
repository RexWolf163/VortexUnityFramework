using System;
using UnityEngine;
using Vortex.Chains.ChainsInfo;
using Vortex.Unity.Chains.Controllers;

namespace Vortex.Chains.Model
{
    public class Chain : IChain
    {
        public event Action OnComplete;

        private readonly ChainData _data;

        internal int CurrentStage = 0;

        /// <summary>
        /// Кешированные этапы цепочки
        /// </summary>
        private ChainStage[] _stages;

        public Chain(ChainData data)
        {
            _data = data;
        }

        /// <summary>
        /// Этапы цепочки
        /// </summary>
        internal ChainStage[] Stages
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    _stages = null;
#endif
                return _stages ?? _data.Stages;
            }
        }

        public void Init()
        {
            if (Stages.Length == 0)
            {
                Debug.LogError($"[ChainStage: {_data.GetName()}] Empty chain");
                return;
            }

            CurrentStage = 0;
            this.RunCurrentStage();
        }

        public void Destroy()
        {
            this.StopCurrentStage();
        }

        public ChainInfo GetInfo()
        {
            //TODO
            return null;
        }

        internal void Complete() => OnComplete?.Invoke();
    }
}