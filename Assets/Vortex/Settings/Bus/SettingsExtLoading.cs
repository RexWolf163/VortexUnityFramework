using System;
using Vortex.Core.AppLoading.Base;
using Vortex.Core.AppLoading.Base.SystemController;

namespace Vortex.Settings.Bus
{
    public partial class Settings : SystemController<Settings>
    {
        public override LoadingData GetAgentLoadingData() => null;

        public override Type[] GetAgentWaitingFor() => Type.EmptyTypes;
    }
}