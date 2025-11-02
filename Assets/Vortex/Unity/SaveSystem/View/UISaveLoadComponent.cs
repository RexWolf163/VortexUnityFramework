using System;
using System.Collections;
using UnityEngine;
using Vortex.Core.LocalizationSystem;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.SaveSystem.Model;
using Vortex.Core.System.ProcessInfo;
using Vortex.Unity.LocalizationSystem;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UIProviderSystem.View;

namespace Vortex.Unity.SaveSystem.View
{
    public class UISaveLoadComponent : MonoBehaviour
    {
        [SerializeField] private UserInterface userInterface;

        private ProcessData _processData;
        private ProcessData _fullProcessData;

        [SerializeField] private UIComponent title;

        [SerializeField] private UIComponent progress;

        [SerializeField, LocalizationKey] private string loadingText;
        [SerializeField, LocalizationKey] private string savingText;

        [SerializeField, LocalizationKey] private string progressTextPattern;

        private bool _process;

        private void OnEnable()
        {
            StartCoroutine(Run());
        }

        private void OnDisable()
        {
            _process = false;
            StopAllCoroutines();
        }

        private IEnumerator Run()
        {
            (_fullProcessData, _processData) = SaveController.GetProcessData();
            _process = true;
            title.SetText(SaveController.State == SaveControllerStates.Loading ? loadingText : savingText);
            yield return null;
            while (_process)
            {
                var progressValue = Math.Floor(100f * _processData.Progress / _processData.Size);
                progress.SetText(string.Format(progressTextPattern.Translate(),
                    _fullProcessData.Progress,
                    _fullProcessData.Size,
                    _processData.Name,
                    progressValue));
                yield return null;
            }

            yield return null;
        }
    }
}