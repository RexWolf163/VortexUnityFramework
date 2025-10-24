using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.BehaviorLogics
{
    public class SimpleInterfaceBehavior : UserInterfaceBehavior
    {
        /// <summary>
        /// Перечень необходимых панелей, которые должны отображаться вместе с этим UI
        /// </summary>
        [SerializeReference, ValueDropdown("@DropDawnHandler.GetTypesNameList<UserInterface>()")]
        private string[] _needPanels = new string[0];

        private List<Type> NeedPanels;

        public override void Init(UserInterface userInterface)
        {
            UI = userInterface;

            UI.OnStateChanged += OnStateChange;
        }

        private void OnStateChange(UserInterfaceStates state)
        {
            if (state != UserInterfaceStates.Showing)
                return;

            if (NeedPanels == null)
            {
                NeedPanels = new List<Type>();
                foreach (var panelName in _needPanels)
                {
                    var type = Type.GetType(panelName);
                    if (type == null)
                    {
                        Debug.LogError($"Can't find Ui by {panelName} type");
                        continue;
                    }

                    var panel = Activator.CreateInstance(type) as UserInterface;
                    if (panel == null)
                    {
                        Debug.LogError($"{type.Name} is not a UserInterface");
                        continue;
                    }

                    NeedPanels.Add(type);
                }
            }

            foreach (var panel in NeedPanels)
                UIProvider.OpenUI(panel);
        }

        public override void DeInit()
        {
        }
    }
}