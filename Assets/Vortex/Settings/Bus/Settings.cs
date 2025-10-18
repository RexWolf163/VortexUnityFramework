using Vortex.Settings.Model;

namespace Vortex.Settings.Bus
{
    public partial class Settings
    {
        private static SettingsModel _data;

        private partial SettingsModel GetData();
        public static SettingsModel Data() => Instance.GetData();
    }
}