namespace AppScripts.Narrative.Model
{
    /// <summary>
    /// Модель этапа диалога
    /// Содержит текст, персонажей участников и теги
    /// Первый персонаж из списка - спикер. остальные для информации
    /// </summary>
    public class NrDialogueStage
    {
        public NrDialogueStage(string text, string[] tags, string[] characters)
        {
            Text = text;
            Tags = tags;
            Characters = characters;
        }

        public string Text { get; }
        public string[] Tags { get; }

        public string[] Characters { get; }
    }
}