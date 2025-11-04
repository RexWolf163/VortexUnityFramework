using Vortex.Core.LogicChainsSystem.Model;
using Vortex.Unity.Extensions.Attributes;

namespace Vortex.Unity.LogicConditionsSystem.Conditions
{
    [FoldoutClass("@ConditionName")]
    public abstract class UnityCondition : Condition
    {
        protected abstract string ConditionName { get; }
    }
}