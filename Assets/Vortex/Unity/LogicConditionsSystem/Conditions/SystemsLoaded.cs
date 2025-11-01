﻿using Sirenix.OdinInspector;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.System.Enums;

namespace Vortex.Unity.LogicConditionsSystem.Conditions
{
    public class SystemsLoaded : UnityCondition
    {
        protected override void Start()
        {
            if (Check())
            {
                Complete();
                return;
            }

            App.OnStateChanged += OnStateChanged;
        }

        public override void DeInit()
        {
            App.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(AppStates newState)
        {
            if (!Check())
                return;
            Complete();
        }

        public override bool Check() => App.GetState() == AppStates.Running;

        [ShowInInspector, DisplayAsString, HideLabel]
        protected override string ConditionName => "Wait all systems loading";
    }
}