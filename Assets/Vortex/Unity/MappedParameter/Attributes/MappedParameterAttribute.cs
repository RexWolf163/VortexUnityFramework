using System;
using UnityEngine;

namespace Vortex.Unity.MappedParameter.Attributes
{
    public class MappedParameterAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        internal Type PresetType { get; private set; }

        public MappedParameterAttribute(Type type)
        {
            PresetType = null;
            if (type.IsInterface || type.IsAbstract)
                return;
            if (!typeof(IMappedParametersGroup).IsAssignableFrom(type))
                return;
            PresetType = type;
        }
#endif
    }
}