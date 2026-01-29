using System;
using UnityEngine;

namespace Vortex.Unity.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ClassFilterAttribute : PropertyAttribute
    {
        public Type RequiredType { get; }

        public ClassFilterAttribute(Type requiredType)
        {
            if (requiredType == null)
                throw new ArgumentNullException(nameof(requiredType));

            if (!requiredType.IsClass && !requiredType.IsInterface)
                throw new ArgumentException("Тип должен быть классом или интерфейсом.", nameof(requiredType));

            RequiredType = requiredType;
        }
    }
}