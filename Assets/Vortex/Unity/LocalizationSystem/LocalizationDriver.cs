using System;
using Vortex.Core.LocalizationSystem;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.LocalizationSystem
{
    public partial class LocalizationDriver : Singleton<LocalizationDriver>, IDriver
    {
        private const string Path = "Localization";

        public event Action OnInit;

        public void Init()
        {
        }

        public void Destroy()
        {
        }

        private void LoadData()
        {
        }
    }
}