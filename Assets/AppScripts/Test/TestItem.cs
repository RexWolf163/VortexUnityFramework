using System;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Model;

namespace AppScripts.Test
{
    [Serializable]
    public class TestItem : Record
    {
        [field: SerializeField] public string Chain { get; protected set; }
    }
}