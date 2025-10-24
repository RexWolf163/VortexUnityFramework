using System;
using UnityEngine;

namespace Vortex.Unity.UIProviderSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class UserInterfaceAttribute : PropertyAttribute
    {
    }
}