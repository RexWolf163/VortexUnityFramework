using UnityEngine;
using Vortex.Unity.UI.Attributes;
using Vortex.Unity.UI.StateSwitcher;

namespace AppScripts.Navigator.Handlers
{
    public class HomePageSwitcher : MonoBehaviour
    {
        private enum States
        {
            Home,
            Page
        }

        [SerializeField, StateSwitcher(typeof(States))]
        private UIStateSwitcher uiStateSwitcher;

        private void OnEnable()
        {
            NavigatorController.OnChangePage += CheckPage;
            uiStateSwitcher.Reset();
            CheckPage();
        }

        private void OnDisable()
        {
            NavigatorController.OnChangePage -= CheckPage;
        }

        private void CheckPage()
        {
            uiStateSwitcher.Set(NavigatorController.IsHome() ? States.Home : States.Page);
        }
    }
}