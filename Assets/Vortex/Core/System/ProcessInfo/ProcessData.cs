namespace Vortex.Core.System.ProcessInfo
{
    public class ProcessData
    {
        public string Name;
        public int Progress;
        public int Size;

        public ProcessData()
        {
        }

        public ProcessData(string name)
        {
            Name = name;
        }
    }
}