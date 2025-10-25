using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Enums;
using Vortex.Unity.UI.Editor.Attributes;
using Vortex.Unity.UI.StateSwitcher;
using Vortex.Unity.UI.UIComponents;

namespace Vortex.Unity.Components.LoaderSystem
{
    public class LoaderView : MonoBehaviour
    {
        private enum States
        {
            Waiting,
            Loading,
            Completed,
        }

        [SerializeField, StateSwitcher(typeof(States))]
        private UIStateSwitcher _switcher;

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
                    StartCoroutine(View());
                    _switcher.Set(States.Loading);
                    return;
                case AppStates.Running:
                    StopAllCoroutines();
                    Refresh();
                    _switcher.Set(States.Completed);
                    App.OnStateChanged -= OnStateChange;
                    return;
                default:
                    _switcher.Set(States.Waiting);
                    break;
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
    }
}