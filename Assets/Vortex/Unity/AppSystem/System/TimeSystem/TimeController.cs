using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.AppSystem.System.TimeSystem
{
    /// <summary>
    /// Класс таймера.
    /// Центральный диспетчер для вызова экшенов по времени
    /// </summary>
    public class TimeController : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Синхронизатор времени для дебага или иного чего 
        /// </summary>
        public static event Action TimeSync;

        #endregion

        #region Params

        /// <summary>
        /// Тиков в секунду
        /// </summary>
        internal const double TicksPerSecond = 10000000;

        /// <summary>
        /// Шаг проверки очереди экшенов.
        /// Чтобы разгрузить проц от проверки на каждом кадре
        /// </summary>
        private const float StepTime = 0.1f;

        /// <summary>
        /// Отметка времени последней проверки очереди 
        /// </summary>
        private static double _lastCheckTime = -1;

        // Переиспользуемый буффер, избавляемся от пересоздания списков
        private static readonly List<Action> ReadyQueue = new();

        /// <summary>
        /// Очередь "следующей волны"
        /// Используется для экшенов, которые откладываются через Accumulate
        /// </summary>
        private static readonly Dictionary<object, Action> NextWaveQueue = new();

        /// <summary>
        /// Очередь на срабатывание
        /// </summary>
        [ShowInInspector, HideInEditorMode] private static List<QueuedAction> _anonymousQueue = new();

        /// <summary>
        /// Очередь на срабатывание без указанного владельца
        /// </summary>
        [ShowInInspector, HideInEditorMode] private static Dictionary<object, QueuedAction> _queue = new();

        /// <summary>
        /// Следующий таймер для обработки
        /// </summary>
        private static double _nextTimer = double.MaxValue;

        #endregion

        #region Public

        /// <summary>
        /// Текущая дата.
        /// Кешируем, чтобы не поменялась на протяжении кадра
        /// (на всякий случай)
        /// </summary>
        public static DateTime Date { get; private set; }

        /// <summary>
        /// Текущее время в секундах.
        /// Два знака после запятой
        /// </summary>
        public static double Time { get; private set; }

        /// <summary>
        /// Отметка времени приложения
        /// UNIX время
        /// </summary>
        public static long Timestamp
        {
            get
            {
                if (Date.Year <= 1)
                    return 0;
                return new DateTimeOffset(Date).ToUnixTimeMilliseconds();
            }
        }

        /// <summary>
        /// Отложенный на конец кадра вызов экшена
        /// </summary>
        /// <param name="action">Отложенный экшен</param>
        public static void Call(Action action) => Call(action, 0, null);

        /// <summary>
        /// Отложенный на конец кадра вызов экшена
        /// </summary>
        /// <param name="action">Отложенный экшен</param>
        /// <param name="owner">
        /// Владелец запроса. Если null, экшен будет без владельца и не может быть отменен.
        /// Если указан владелец - все предыдущие вызовы того же владельца будут перезаписаны.
        /// </param>
        public static void Call(Action action, object owner) => Call(action, 0, owner);

        /// <summary>
        /// Отложенный вызов экшена
        /// </summary>
        /// <param name="action">Отложенный экшен</param>
        /// <param name="stepSecs">Через сколько секунд вызвать</param>
        /// <param name="owner">
        /// Владелец запроса. Если null, экшен будет без владельца и не может быть отменен.
        /// Если указан владелец - все предыдущие вызовы того же владельца будут перезаписаны.
        /// </param>
        public static void Call(Action action, float stepSecs = 0, object owner = null)
        {
            if (action == null)
            {
                if (owner != null && _queue.ContainsKey(owner))
                    _queue.Remove(owner);
                return;
            }

            if (stepSecs <= 0f)
                _lastCheckTime = Time - StepTime;

            _nextTimer = Math.Min(_nextTimer, Time + stepSecs);
            var triggerTime = Time + stepSecs;

            if (owner == null)
            {
                // удалено .Clone()
                // делегаты в C# неизменяемы, Clone() создаёт лишние вызовы
                _anonymousQueue.Add(new QueuedAction
                {
                    Owner = null,
                    Action = action,
                    Timestamp = triggerTime
                });
                return;
            }

            if (_queue.ContainsKey(owner))
                _queue[owner].Set(action, triggerTime);
            else
            {
                _queue.Add(owner, new QueuedAction
                {
                    Owner = owner,
                    Action = action,
                    Timestamp = triggerTime
                });
            }
        }

        /// <summary>
        /// Аккумулировать однотипные вызовы на "следующую волну" 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="owner"></param>
        public static void Accumulate(Action action, object owner)
        {
            if (NextWaveQueue.ContainsKey(owner))
            {
                NextWaveQueue[owner] = action;
                return;
            }

            NextWaveQueue.Add(owner, action);
        }

        /// <summary>
        /// Удалить из очереди экшен указанного владельца
        /// </summary>
        /// <param name="owner">Владелец запроса</param>
        public static void RemoveCall(object owner)
        {
            _queue.Remove(owner);
            NextWaveQueue.Remove(owner);
        }

        /// <summary>
        /// Преобразует секунды в DateTime в локальном часовом поясе
        /// </summary>
        /// <param name="seconds">Отметка времени в формате приложения (секунды)</param>
        public static DateTime DateFromSeconds(long seconds)
        {
            // Unix-время отсчитывается с 1 января 1970 года (эпоха Unix)
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Добавляем количество секунд к эпохе и конвертируем в локальное время
            var dateTime = epoch.AddSeconds(seconds);

            // Возвращаем время, скорректированное для локальной часовой зоны
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Преобразует тики в DateTime в локальном часовом поясе
        /// </summary>
        /// <param name="ticks">сколько тиков</param>
        public static DateTime DateFromTicks(long ticks)
        {
            var time = new DateTime(ticks);
            return TimeZoneInfo.ConvertTimeFromUtc(time, TimeZoneInfo.Local);
        }

        #endregion

        #region Private

        [RuntimeInitializeOnLoadMethod]
        private static void AutoCreate()
        {
            var go = Instantiate(new GameObject());
            go.AddComponent<TimeController>();
            go.name = "TimeController";

            SetTimeValue();
        }

        private static void SetTimeValue()
        {
            var now = DateTime.UtcNow;
            Date = now;
            Time = Math.Round(now.Ticks / TicksPerSecond, 2);
        }

        private void Awake() => DontDestroyOnLoad(this);

        /// <summary>
        /// Проверка очереди запросов и активация тех, чье время пришло
        /// </summary>
        private void CheckQueue()
        {
            if (_anonymousQueue.Count == 0 && _queue.Count == 0) return;
            if (Time < _nextTimer)
                return;
            _nextTimer = double.MaxValue;

            // Удалены временные списки и пересоздание списков
            // Меньше нагрузка на GC

            ReadyQueue.Clear();

            // Идём с конца, удаляем сразу
            var c = _anonymousQueue.Count - 1;
            for (int i = c; i >= 0; i--)
            {
                //Запускаем актуальные, остальные набиваем в новый список 
                var actionData = _anonymousQueue[i];
                if (actionData.Timestamp <= Time)
                {
                    ReadyQueue.Add(actionData.Action);
                    _anonymousQueue.RemoveAt(i);
                }
                else
                    _nextTimer = Math.Min(_nextTimer, actionData.Timestamp);
            }

            // Идём с конца, удаляем сразу
            var keys = _queue.Keys.ToArray();
            foreach (var key in keys)
            {
                //Запускаем актуальные, остальные набиваем в новый список 
                var actionData = _queue[key];
                if (actionData.Timestamp <= Time)
                {
                    ReadyQueue.Add(actionData.Action);
                    _queue.Remove(key);
                }
                else
                    _nextTimer = Math.Min(_nextTimer, actionData.Timestamp);
            }

            // восстанавливаем оригинальный порядок
            ReadyQueue.Reverse();

            foreach (var action in ReadyQueue)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Сигнал синхронизации кадра
        /// </summary>
        private void Update() => TimeSync?.Invoke();

        /// <summary>
        /// Обновляем данные времени и запускаем проверку очереди,
        /// если с последней проверки прошло больше или равно шагу проверки
        /// </summary>
        private void LateUpdate()
        {
            SetTimeValue();
            //Запуск отложенной волны, если корректный ее запуск пропущен
            if (NextWaveQueue.Count > 0)
                RunNextWave();
            if (Time - _lastCheckTime < StepTime)
                return;
            _lastCheckTime = Time;
            CheckQueue();
        }

        /// <summary>
        /// Запуск экшенов "следующей волны", отложенных через Accumulate
        /// </summary>
        private static void RunNextWave()
        {
            if (NextWaveQueue.Count == 0)
                return;

            ReadyQueue.Clear();
            ReadyQueue.AddRange(NextWaveQueue.Values);
            NextWaveQueue.Clear();

            foreach (var action in ReadyQueue)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        #endregion
    }
}