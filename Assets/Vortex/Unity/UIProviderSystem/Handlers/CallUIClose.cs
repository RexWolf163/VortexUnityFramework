using UnityEngine;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.View;

namespace Vortex.Unity.UIProviderSystem.Handlers
{
    public class CallUIClose : MonoBehaviour
    {
        [SerializeField] protected UserInterface ui;

        [SerializeField] private UIComponent uiComponent;

        private void Awake() => uiComponent?.SetAction(CloseUI);

        private void CloseUI()
        {
            UIProvider.Close(ui.GetId());
        }
    }
}