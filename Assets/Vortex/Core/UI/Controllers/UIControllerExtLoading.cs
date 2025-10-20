using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.UI.Interfaces;

namespace Vortex.Core.UI.Controllers
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// Функционал загрузки и регистрации
    /// </summary>
    public static partial class UIController
    {
        /// <summary>
        /// Регистрация нового интерфейса в индексе
        /// </summary>
        /// <param name="ui"></param>
        public static void Register(IUserInterface ui)
        {
            _uis.AddNew(ui.GetType(), ui);
        }

        /// <summary>
        /// Снятие с регистрации интерфейса
        /// </summary>
        /// <param name="ui"></param>
        public static void Unregister(IUserInterface ui)
        {
            var type = ui.GetType();
            if (!_uis.ContainsKey(type))
            {
                Debug.LogError($"[UIController] UI doesn't exist: {type}");
                return;
            }

            _uis.Remove(type);
        }
    }
}