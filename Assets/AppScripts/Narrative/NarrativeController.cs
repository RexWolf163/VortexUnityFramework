using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppScripts.Narrative.Model;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.ProcessInfo;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.SettingsSystem.Bus;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.DatabaseSystem;

namespace AppScripts.Narrative
{
    /// <summary>
    /// Контроллер работы с нарративом.
    /// Нарратив может быть двух типов - кодекс или диалоги.
    /// Кодекс нужно рассматривать широко, как справочный интерфейс в рамках которого
    /// можно представлять и инвентарь и справочную информацию
    ///
    /// Работа с диалогом идет по event.
    /// Работа с кодексом идет по фактическому запросу.
    /// </summary>
    public class NarrativeController : Singleton<NarrativeController>, IProcess
    {
        /// <summary>
        /// Событие начала или завершения диалога
        /// </summary>
        public static event Action OnDialogueStateChanged;

        /// <summary>
        /// Событие начала сюжета
        /// </summary>
        public static event Action OnStart;

        /// <summary>
        /// Событие завершения сюжета
        /// </summary>
        public static event Action OnComplete;

        public enum NarrativeType
        {
            Dialogue,
            Codex
        }

        /// <summary>
        /// Индекс персонажей
        /// </summary>
        private static SortedDictionary<string, NrCharacterModel> _index = new();

        /// <summary>
        /// Индекс для поиска ключа ассоциации в БД по техническому имени
        /// </summary>
        private static SortedDictionary<string, string> _reverseIndex = new();

        /// <summary>
        /// Начальная точка просмотра кодекса
        /// </summary>
        private static ArticyRef _codexPoint;

        /// <summary>
        /// Начальные точки сюжетных частей
        /// </summary>
        private static ArticyRef[] _parts;

        /// <summary>
        /// Точка текущего диалога (внутри сюжетной части)
        /// </summary>
        private static ArticyRef _currentDialogue;

        /// <summary>
        /// Сейчас должен отыгрываться диалог
        /// </summary>
        public static bool HasDialogue { get; private set; }

        /// <summary>
        /// Реестр стейтмашин нарратива
        /// При регистрации на заполненный слот должно выдаваться сообщение-варнинг
        /// </summary>
        private static SortedDictionary<NarrativeType, ArticyPlayer> _articyPlayers = new();

        #region Init/Load

        [RuntimeInitializeOnLoadMethod]
        private static void Run()
        {
            Loader.Register<NarrativeController>();
            Settings.OnInit += OnSettingsInit;
        }

        private static void OnSettingsInit()
        {
            _codexPoint = Settings.Data().NarrativeCodex;
            if (_codexPoint == null)
            {
                Debug.LogError("[NarrativeController] No narrative codex defined.");
                return;
            }

            _parts = Settings.Data().NarrativeParts;
            if (_parts == null || _parts.Length == 0)
            {
                Debug.LogError("[NarrativeController] No narrative parts defined.");
                return;
            }

            _currentDialogue = _parts[0];
        }

        private ProcessData _processData = new()
        {
            Name = "NarrativeSystem",
            Progress = 0,
            Size = 100
        };

        public ProcessData GetProcessInfo() => _processData;

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _processData.Progress = 0;
            var characters = Database.GetRecords<NrCharacterModel>();
            _processData.Size = characters.Count;
            for (var i = 0; i < _processData.Size; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _processData.Progress++;
                _index.Add(characters[i].GuidPreset, characters[i]);

                await Task.Yield();
            }

            TimeController.Accumulate(() =>
            {
                var list = _index.Values;
                foreach (var artObj in list)
                    _reverseIndex.Add(artObj.Ref.GetObject().TechnicalName, artObj.GuidPreset);
            }, this);

            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => new Type[] { typeof(DatabaseDriver) };

        #endregion

        /// <summary>
        /// Регистрация стейтмашины потока нарратива
        /// </summary>
        /// <param name="articyPlayer">контроллер стейтмашины</param>
        public static void RegisterPlayer(ArticyPlayer articyPlayer)
        {
            var narrativeType = articyPlayer.GetNarrativeType();
            if (!_articyPlayers.TryGetValue(narrativeType, out var oldArticyPlayer))
            {
                _articyPlayers.Add(narrativeType, articyPlayer);
            }
            else
            {
                if (oldArticyPlayer != null)
                    Debug.LogWarning(
                        $"[Narrative Controller] ArticyPlayer for {narrativeType} was set early. Link was rewrite now.");
            }

            _articyPlayers[narrativeType] = articyPlayer;
            articyPlayer.SetPoint(_currentDialogue);

            if (narrativeType != NarrativeType.Dialogue)
                return;

            if (!HasDialogue)
                HasDialogue = true;

            OnDialogueStateChanged?.Invoke();
        }

        /// <summary>
        /// Снятие стейт-машины с регистрации
        /// </summary>
        /// <param name="articyPlayer"></param>
        public static void UnregisterPlayer(ArticyPlayer articyPlayer)
        {
            var narrativeType = articyPlayer.GetNarrativeType();
            if (!_articyPlayers.ContainsKey(narrativeType))
                return;
            _articyPlayers[narrativeType] = null;
        }

        public static NrDialogueStage GetDialogueData()
        {
            if (!_articyPlayers.ContainsKey(NarrativeType.Dialogue) || _articyPlayers[NarrativeType.Dialogue] == null)
            {
                Debug.LogError("[NarrativeController] No dialogue machine defined.");
                return null;
            }

            var player = _articyPlayers[NarrativeType.Dialogue];

            var flowPoint = player.GetCurrentPoint();
            var text = "";
            var menuText = "";
            var characters = new List<string>();
            var tags = new string[0];
            if (flowPoint == null)
                return null;
            if (flowPoint.Point is IObjectWithLocalizableText ot)
                text = ot.Text;
            if (flowPoint.Point is IObjectWithTarget trg)
                characters.Add(_reverseIndex[trg.Target.TechnicalName]);

            return new NrDialogueStage(text, tags, characters.ToArray());
        }


        /// <summary>
        /// тестовый метод запуска
        /// TODO доработать логику
        /// </summary>
        public static void Start()
        {
            _currentDialogue = _parts[0];
            if (_articyPlayers.ContainsKey(NarrativeType.Dialogue) && _articyPlayers[NarrativeType.Dialogue] != null)
            {
                _articyPlayers[NarrativeType.Dialogue].SetPoint(_currentDialogue);

                HasDialogue = true;
            }

            OnStart?.Invoke();
        }
    }
}