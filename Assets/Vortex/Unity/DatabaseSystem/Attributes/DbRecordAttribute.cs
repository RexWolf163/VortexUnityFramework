using System;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Presets;

namespace Vortex.Unity.DatabaseSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class DbRecordAttribute : PropertyAttribute
    {
        public Type RecordType { get; private set; }

        public DbRecordAttribute(Type type)
        {
            RecordType = type;
        }

        public DbRecordAttribute()
        {
            RecordType = typeof(IRecordPreset);
        }
    }
}