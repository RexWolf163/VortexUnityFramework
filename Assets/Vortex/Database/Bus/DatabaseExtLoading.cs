using System;
using Vortex.Core.AppLoading.Base;
using Vortex.Core.AppLoading.Base.SystemController;

namespace Vortex.Database.Bus
{
    public partial class Database : SystemController<Database>
    {
        public override LoadingData GetAgentLoadingData()
        {
            return _loadingData ??= new LoadingData
            {
                Name = "Database",
                Progress = 0,
                Size = 0
            };
        }

        public override Type[] GetAgentWaitingFor() => Type.EmptyTypes;
    }
}