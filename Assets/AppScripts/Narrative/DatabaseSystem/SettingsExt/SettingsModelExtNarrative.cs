using Articy.Unity;

namespace Vortex.Core.SettingsSystem.Model
{
    public partial class SettingsModel
    {
        public ArticyRef NarrativeCodex { get; private set; }
        public ArticyRef[] NarrativeParts { get; private set; }
    }
}