namespace Vortex.Core.System.Abstractions
{
    public interface IDataStorage
    {
        public T GetData<T>() where T : class;
    }
}