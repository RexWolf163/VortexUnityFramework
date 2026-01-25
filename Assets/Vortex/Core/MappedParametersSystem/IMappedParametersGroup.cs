namespace Vortex.Core.MappedParametersSystem
{
    public interface IMappedParametersGroup
    {
        public string[] GetParametersList();
        public int GetValue(string paramName);
    }
}