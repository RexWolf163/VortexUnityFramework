using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.BehaviorLogics
{
    /// <summary>
    /// Обычный интерфейс.
    /// Можно настроить те UI которые должны быть открыты вместе с ним
    /// </summary>
    public class SimpleInterfaceBehavior : UserInterfaceBehavior
    {
        /// <summary>
        /// Перечень необходимых панелей, которые должны отображаться вместе с этим UI
        /// </summary>
        [SerializeReference, ValueDropdown("@DropDawnHandler.GetTypesNameList<UserInterface>()")]
        private string[] _needPanels = new string[0];

        private Type[] _needPanelsCash;

        public override void Init(UserInterface userInterface)
        {
            InitNeedPanelsCash();
            UI = userInterface;
            UI.OnStateChanged += OnStateChange;
        }

        public override void DeInit()
        {
            if (UI != null)
                UI.OnStateChanged -= OnStateChange;
        }

        /// <summary>
        /// Обработка изменения состояния управляемого интерфейса
        /// Если он открывается, то нужно запросить открытие связанных ui
        /// </summary>
        /// <param name="state"></param>
        private void OnStateChange(UserInterfaceStates state)
        {
            if (state != UserInterfaceStates.Showing)
                return;

            UIProvider.OpenUI(_needPanelsCash);
        }

        /// <summary>
        /// Инициализация списка необходимых UI, которые должны открываться вместе с ним
        /// </summary>
        private void InitNeedPanelsCash()
        {
            if (_needPanelsCash == null)
            {
                _needPanelsCash = new Type[_needPanels.Length];
                for (var i = _needPanels.Length - 1; i >= 0; i--)
                {
                    var panelName = _needPanels[i];
                    var type = Type.GetType(panelName);
                    if (type == null)
                    {
                        Debug.LogError($"Can't find Ui by {panelName} type");
                        continue;
                    }

                    if (!type.IsSubclassOf(typeof(UserInterface)))
                    {
                        Debug.LogError($"{type.Name} is not a UserInterface");
                        continue;
                    }

                    _needPanelsCash[i] = type;
                }
            }
        }
    }
}