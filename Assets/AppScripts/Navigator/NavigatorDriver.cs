using System;
using System.Collections.Generic;
using Vortex.Core.System.Abstractions;

namespace AppScripts.Navigator
{
    public class NavigatorDriver : ISystemDriver
    {
        public event Action OnInit;

        private SortedDictionary<string, NavigatorPage> _index = new();

        public void Init()
        {
            //TODO Load data
        }

        public void Destroy()
        {
        }

        internal void SetLink(SortedDictionary<string, NavigatorPage> index) => _index = index;
    }
}