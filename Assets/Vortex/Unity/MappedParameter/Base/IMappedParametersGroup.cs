namespace Vortex.Unity.MappedParameter
{
    public interface IMappedParametersGroup
    {
#if UNITY_EDITOR
        public string[] GetParametersList();
#endif
    }
}