using Articy.Unity;
using Vortex.Core.DatabaseSystem.Model;

namespace AppScripts.Narrative.Model
{
    public class NrCharacterModel : Record
    {
        public ArticyRef Ref { get; protected set; }

        public override string GetDataForSave()
        {
            return string.Empty;
        }

        public override void LoadFromSaveData(string data)
        {
            //Ignore
        }
    }
}