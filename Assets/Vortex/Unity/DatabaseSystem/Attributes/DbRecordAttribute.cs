using System;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;

namespace Vortex.Unity.DatabaseSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class DbRecordAttribute : PropertyAttribute
    {
        public Type RecordClass { get; private set; }
        public RecordTypes? RecordType { get; private set; }


        public DbRecordAttribute(Type @class)
        {
            RecordClass = @class;
            RecordType = null;
        }

        public DbRecordAttribute(RecordTypes recordType)
        {
            RecordClass = typeof(IRecordPreset);
            RecordType = recordType;
        }

        public DbRecordAttribute(Type @class, RecordTypes recordType)
        {
            RecordClass = @class;
            RecordType = recordType;
        }

        public DbRecordAttribute()
        {
            RecordClass = typeof(IRecordPreset);
            RecordType = null;
        }
    }
}