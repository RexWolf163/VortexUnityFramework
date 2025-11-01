using Vortex.Core.LogicChainsSystem.Model;
using Vortex.Unity.Extensions.Attributes;

namespace Vortex.Unity.LogicChainsSystem.Actions
{
    [FoldoutClass("@NameAction")]
    public abstract class UnityLogicAction : LogicAction
    {
        protected abstract string NameAction { get; }
    }
}