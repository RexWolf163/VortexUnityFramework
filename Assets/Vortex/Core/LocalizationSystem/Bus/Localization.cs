using Vortex.Core.System.Abstractions;

namespace Vortex.Core.LocalizationSystem.Bus
{
    public class Localization : SystemController<Localization, IDriver>
    {
        protected override void OnDriverConnect()
        {
        }

        protected override void OnDriverDisonnect()
        {
        }
    }
}