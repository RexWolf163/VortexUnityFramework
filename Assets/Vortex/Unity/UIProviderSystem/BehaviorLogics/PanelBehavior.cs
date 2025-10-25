using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.BehaviorLogics
{
    public class PanelBehavior : UserInterfaceBehavior
    {
        public override void Init(UserInterface userInterface)
        {
            UI = userInterface;
            UIProvider.OnOpen += OnWndChanges;
            UIProvider.OnClose += OnWndChanges;
            OnWndChanges(null);
        }

        public override void DeInit()
        {
            UIProvider.OnOpen -= OnWndChanges;
            UIProvider.OnClose -= OnWndChanges;
        }

        /// <summary>
        /// Обработка изменения состояния управляемого интерфейса
        /// Если он открывается, то нужно запросить открытие связанных ui
        /// </summary>
        /// <param name="userInterface"></param>
        private void OnWndChanges(UserInterface userInterface)
        {
            if (userInterface != null && userInterface.GetBehaviorType() == typeof(PanelBehavior))
                return;
            if (userInterface == UI)
                return;
            var list = UIProvider.GetAllOpenedUis<SimpleInterfaceBehavior>();
            if (list.Count > 0)
                UIProvider.CloseUI(UI.GetType());
            else
                UIProvider.OpenUI(UI.GetType());
        }
    }
}