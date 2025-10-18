using UnityEngine;
using Vortex.Core.App;
using Vortex.Core.Enums;

namespace Vortex.Chains.Model
{
    public class WaitingAppState : ChainCondition
    {
        [SerializeField] private AppStates _state;

        public override void Check()
        {
            var state = AppController.Data.GetState();
            if (state == _state)
            {
                Complete();
                return;
            }

            AppController.Data.OnStateChanged += CheckState;
        }

        public override void Cancel()
        {
            AppController.Data.OnStateChanged -= CheckState;
        }

        private void CheckState(AppStates state)
        {
            if (state != _state)
                return;

            AppController.Data.OnStateChanged -= CheckState;
            Complete();
        }
    }
}