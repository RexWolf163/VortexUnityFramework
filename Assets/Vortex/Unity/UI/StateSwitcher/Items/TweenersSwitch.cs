using UnityEngine;
using Vortex.Unity.UI.Tweeners;

namespace Vortex.Unity.UI.StateSwitcher.Items
{
    public class TweenersSwitch : StateItem
    {
        [SerializeField] private TweenerBase[] _tweeners;

        public override void Set()
        {
            if(_tweeners == null) return;
            foreach (var tweenerBase in _tweeners)
            {
                tweenerBase?.Forward();
            }
        }

        public override void DefaultState()
        {
            if(_tweeners == null) return;
            foreach (var tweenerBase in _tweeners)
            {
                tweenerBase?.Back();
            }
        }

#if UNITY_EDITOR

        public override string DropDownItemName => "Switch Tweeners";
        public override string DropDownGroupName => "Graphics";
        public override StateItem Clone()
        {
            return new TweenersSwitch
            {
                _tweeners = _tweeners
            };
        }
#endif
    }
}