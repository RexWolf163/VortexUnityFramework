using System;
using Vortex.Unity.Extensions.Attributes;

namespace Vortex.Unity.UI.StateSwitcher
{
    /// <summary>
    /// Класс элемента состояния.
    /// Можно при наследовании расширять функционал для добавления управляемых элементов
    /// </summary>
    [Serializable]
    [FoldoutClass("$DropDownItemName")]
    public abstract class StateItem
    {
        public abstract void Set();

        public abstract void DefaultState();

#if UNITY_EDITOR
        public abstract string DropDownItemName { get; }
        public abstract string DropDownGroupName { get; }
        public abstract StateItem Clone();
#endif
    }
}