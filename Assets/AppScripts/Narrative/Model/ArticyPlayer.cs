using System.Collections.Generic;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;

namespace AppScripts.Narrative.Model
{
    [RequireComponent(typeof(ArticyFlowPlayer))]
    public class ArticyPlayer : MonoBehaviour, IArticyFlowPlayerCallbacks
    {
        [SerializeField] private NarrativeController.NarrativeType narrativeType;

        [SerializeField] private ArticyFlowPlayer flowPlayer;

        private IFlowObject currentObject;

        private IList<Branch> branches;

        public void OnFlowPlayerPaused(IFlowObject aObject) => currentObject = aObject;

        public void OnBranchesUpdated(IList<Branch> aBranches) => branches = aBranches;

        private void OnEnable()
        {
            TimeController.Call(() =>
                NarrativeController.RegisterPlayer(this), 0, this);
        }

        private void OnDisable()
        {
            NarrativeController.UnregisterPlayer(this);
            TimeController.RemoveCall(this);
        }

        internal void FlowPlay()
        {
        }

        /// <summary>
        /// Тип обрабатываемого нарратива
        /// </summary>
        /// <returns></returns>
        public NarrativeController.NarrativeType GetNarrativeType() => narrativeType;

        /// <summary>
        /// Выставление начальной точки воспроизведения потока
        /// </summary>
        /// <param name="flowPoint"></param>
        public void SetPoint(ArticyRef flowPoint)
        {
            flowPlayer.StartOn = flowPoint.GetObject();
            flowPlayer.Play();
        }

        /// <summary>
        /// Возвращает текущую точку потока.
        /// </summary>
        /// <returns></returns>
        public NrFlowPoint GetCurrentPoint()
        {
            if (currentObject == null || branches == null)
                return null;
            
            return new NrFlowPoint(currentObject, branches);
        }
    }
}