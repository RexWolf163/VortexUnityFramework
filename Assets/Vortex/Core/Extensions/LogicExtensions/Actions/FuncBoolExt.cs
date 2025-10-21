using System;

namespace Vortex.Core.Extensions.LogicExtensions.Actions
{
    public static class FuncBoolExt
    {
        public static bool Run(this Func<bool> owner)
        {
            foreach (var @delegate in owner.GetInvocationList())
                if (!(bool)@delegate.DynamicInvoke())
                    return false;

            return true;
        }
    }
}