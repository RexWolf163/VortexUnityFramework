using Sirenix.OdinInspector;

namespace Vortex.Unity.DatabaseSystem.Editor
{
    public static class DatabaseHandler
    {
        public static ValueDropdownList<string> GetRecords() => DatabaseDriver.Instance.GetDropdownList();
    }
}