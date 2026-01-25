using System;
using UnityEngine;
using Vortex.Core.MappedParametersSystem;

namespace Vortex.Unity.MappedParametersSystem.Attributes
{
    public class MappedParameterAttribute : PropertyAttribute
    {
        internal Type PresetType { get; private set; }

        public MappedParameterAttribute(Type type)
        {
#if UNITY_EDITOR
            PresetType = null;
            if (type.IsInterface || type.IsAbstract)
                return;
            if (!typeof(IMappedParametersGroup).IsAssignableFrom(type))
                return;
            PresetType = type;
#endif
        }
    }
}