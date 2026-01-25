using Sirenix.OdinInspector;
using Vortex.Core.LogicChainsSystem.Model;
using Vortex.Unity.Extensions.Attributes;

namespace Vortex.Unity.LogicConditionsSystem.Conditions
{
    [FoldoutClass("@ConditionName")]
    public abstract class UnityCondition : Condition
    {
        [ShowInInspector, DisplayAsString, HideLabel, PropertyOrder(-100)]
        protected abstract string ConditionName { get; }
    }
}