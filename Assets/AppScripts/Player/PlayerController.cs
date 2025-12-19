using AppScripts.Narrative;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.System.Abstractions;

namespace AppScripts.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        private const string SaveKey = "Save_";

        public static PlayerData Data { get; private set; } = new();

        /// <summary>
        /// Задается слот сохранения.
        /// После этого проводится загрузка прогресса
        /// </summary>
        /// <param name="numberSlot"></param>
        public static void SetSaveSlot(string numberSlot)
        {
            Data.CurrentSaveSlot = numberSlot;
            LoadData();
        }

        /// <summary>
        /// Загрузка прогресса
        /// </summary>
        private static void LoadData()
        {
            var index = SaveController.GetIndex();
            if (index.ContainsKey(Data.CurrentSaveSlot ?? string.Empty))
                SaveController.Load(Data.CurrentSaveSlot);
//TODO
            NarrativeController.Start();
        }
    }
}