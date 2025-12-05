using Vortex.Core.DatabaseSystem.Model;

namespace AppSdk.Comics.Database
{
    public class ComicsData : Record
    {
        public ComicsStage[] ComicsStages { get; protected set; }

        /// <summary>
        /// отметка что комикс был уже показан
        /// </summary>
        public bool WasShowed { get; set; }

        public override string GetDataForSave() => WasShowed ? "Y" : "N";

        public override void LoadFromSaveData(string data) => WasShowed = data == "Y";
    }
}