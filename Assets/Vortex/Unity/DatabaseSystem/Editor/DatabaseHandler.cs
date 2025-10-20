using Sirenix.OdinInspector;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.DatabaseSystem.Model;

namespace Vortex.Unity.DatabaseSystem.Editor
{
    public static class DatabaseHandler
    {
        public static ValueDropdownList<string> GetRecords()
        {
            var result = new ValueDropdownList<string>();
            DatabaseDriver.RefreshDb();

            var list = Database.GetRecords<Record>();
            foreach (var record in list)
                result.Add(record.Name, record.Guid);

            return result;
        }
    }
}