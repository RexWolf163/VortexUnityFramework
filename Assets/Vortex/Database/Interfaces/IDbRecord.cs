namespace Vortex.Core.Database.Record
{
    public interface IDbRecord
    {
        public string GetGuid();

        public string GetName();

        public string ToCsvString();

        public bool FromCsvString(string csvString);

        public string GetCsvTitle();
    }
}