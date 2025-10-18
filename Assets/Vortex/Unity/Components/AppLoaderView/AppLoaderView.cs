using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.App;
using Vortex.Core.Enums;
using Vortex.Core.Loading.Controllers;
using Vortex.UI.Components.UIComponents;

public class AppLoaderView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField, ValueDropdown("AnimatorTriggers")]
    private string _startTrigger;

    [SerializeField, ValueDropdown("AnimatorTriggers")]
    private string _completeTrigger;

    [SerializeField] private UIComponent uiComponent;

    private AppModel _data;

    private void Awake()
    {
        _data = AppController.Data;
        _data.OnStateChanged += OnStateChange;
        OnStateChange(_data.GetState());
    }

    private void OnDestroy()
    {
        _data.OnStateChanged -= OnStateChange;
        StopAllCoroutines();
    }

    private IEnumerator View()
    {
        while (_data.GetState() == AppStates.Starting)
        {
            var loadingData = AppLoader.GetCurrentLoadingData();
            var step = AppLoader.GetProgress();
            var size = AppLoader.GetSize();
            if (loadingData != null)
            {
                var progress = loadingData.Size == 0 ? 0 : Mathf.Floor(100f * loadingData.Progress / loadingData.Size);
                uiComponent.SetText($"{step} from {size}: {loadingData.Name}: {progress}%");
            }

            yield return new WaitForSeconds(.3f);
        }
    }

    private void OnStateChange(AppStates state)
    {
        switch (state)
        {
            case AppStates.Starting:
                _animator.SetTrigger(_startTrigger);
                StartCoroutine(View());
                _animator.SetTrigger(_startTrigger);
                return;
            case AppStates.Running:
                StopAllCoroutines();
                _animator.SetTrigger(_completeTrigger);
                _data.OnStateChanged -= OnStateChange;
                return;
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