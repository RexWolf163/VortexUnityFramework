using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.UIProviderSystem.BehaviorLogics;

namespace Vortex.Unity.UIProviderSystem.Model
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    public abstract partial class UserInterface
    {
        #region Params

        /// <summary>
        /// Скрипт поведения
        /// </summary>
        [SerializeReference, HideLabel,
         TabGroup("Behavior"),
         InfoBox("Укажите тип интерфейса!", InfoMessageType.Warning, VisibleIf = "ShowWarning"),
         GUIColor("GetBehaviorColor")]
        private UserInterfaceBehavior behaviorLogic;

        #endregion

        #region Private

        /// <summary>
        /// Возвращает тип поведения UI
        /// </summary>
        /// <returns></returns>
        internal Type GetBehaviorType() => behaviorLogic.GetType();

        #endregion

#if UNITY_EDITOR
        private string GetBehaviorName() => behaviorLogic?.GetType().Name;

        private bool ShowWarning() => behaviorLogic == null;

        private Color GetBehaviorColor() => behaviorLogic != null ? Color.yellow : Color.red;
#endif
    }
}