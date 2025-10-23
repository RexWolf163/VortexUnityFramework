using System;
using UnityEngine;

namespace Vortex.Unity.DatabaseSystem.Editor.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class DbRecordAttribute : PropertyAttribute
    {
    }
}