using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Enums;
using Vortex.Unity.UI.UIComponents;

namespace Vortex.Unity.Components.LoaderSystem
{
    public class LoaderView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField, ValueDropdown("AnimatorTriggers")]
        private string _startTrigger;

        [SerializeField, ValueDropdown("AnimatorTriggers")]
        private string _completeTrigger;

        [SerializeField] private UIComponent uiComponent;

        private AppStates _state;

        private void Awake()
        {
            App.OnStateChanged += OnStateChange;
            OnStateChange(App.GetState());
        }

        private void OnDestroy()
        {
            App.OnStateChanged -= OnStateChange;
            StopAllCoroutines();
        }

        private IEnumerator View()
        {
            while (_state == AppStates.Starting)
            {
                Refresh();
                yield return new WaitForSeconds(.3f);
            }
        }

        private void OnStateChange(AppStates state)
        {
            _state = state;
            switch (state)
            {
                case AppStates.Starting:
                    _animator.SetTrigger(_startTrigger);
                    StartCoroutine(View());
                    _animator.SetTrigger(_startTrigger);
                    return;
                case AppStates.Running:
                    StopAllCoroutines();
                    Refresh();
                    _animator.SetTrigger(_completeTrigger);
                    App.OnStateChanged -= OnStateChange;
                    return;
            }
        }

        private void Refresh()
        {
            var loadingData = Core.LoaderSystem.Bus.Loader.GetCurrentLoadingData();
            var step = Core.LoaderSystem.Bus.Loader.GetProgress();
            var size = Core.LoaderSystem.Bus.Loader.GetSize();
            if (loadingData != null)
            {
                var progress = loadingData.Size == 0
                    ? 0
                    : Mathf.Floor(100f * loadingData.Progress / loadingData.Size);
                uiComponent.SetText($"{step} from {size}: {loadingData.Name}: {progress}%");
            }
        }

#if UNITY_EDITOR
        private ValueDropdownList<string> AnimatorTriggers()
        {
            var triggers = new ValueDropdownList<string>();
            if (_animator == null)
                return triggers;

            var list = _animator.parameters.Where(parameter =>
                parameter.type == AnimatorControllerParameterType.Trigger);
            foreach (var parameter in list)
                triggers.Add(new ValueDropdownItem<string>(parameter.name, parameter.name));

            return triggers;
        }
#endif
    }
}