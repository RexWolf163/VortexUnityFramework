using System;
using System.Collections.Generic;
using Vortex.Core.MappedParametersSystem;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.MappedParametersSystem
{
    public partial class MappedParametersDriver : Singleton<MappedParametersDriver>, IDriverMappedParameters
    {
        public event Action OnInit;

        private Dictionary<string, IMappedParametersGroup> _indexMaps;

        public void Init()
        {
        }

        public void Destroy()
        {
        }

        public void SetIndex(Dictionary<string, IMappedParametersGroup> indexMaps)
        {
            _indexMaps = indexMaps;
        }
    }
}