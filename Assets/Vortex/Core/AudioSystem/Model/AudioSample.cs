using Vortex.Core.DatabaseSystem.Model;

namespace Vortex.Core.AudioSystem.Model
{
    public abstract class AudioSample : Record
    {
        public override string GetDataForSave() => null;

        public override void LoadFromSaveData(string data)
        {
        }

        public object Sample { get; protected set; }
    }
}