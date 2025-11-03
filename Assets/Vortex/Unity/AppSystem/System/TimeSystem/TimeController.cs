using System;
using System.Collections.Generic;
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
        [ShowInInspector, HideInEditorMode] private static List<QueuedAction> _queue = new();

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
            if (stepSecs <= 0f)
                _lastCheckTime = Time - StepTime;

            var triggerTime = Time + stepSecs;

            if (owner == null)
            {
                // удалено .Clone()
                // делегаты в C# неизменяемы, Clone() создаёт лишние вызовы

                _queue.Add(new QueuedAction
                {
                    Owner = null,
                    Action = action,
                    Timestamp = triggerTime
                });
                return;
            }

            foreach (var queuedAction in _queue)
            {
                if (queuedAction.Owner != owner)
                    continue;

                queuedAction.Set(action, triggerTime);
                return;
            }

            _queue.Add(new QueuedAction
            {
                Owner = owner,
                Action = action,
                Timestamp = triggerTime
            });
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
        /// Запуск экшенов "следующей волны", отложенных через Accumulate
        /// </summary>
        public static void RunNextWave()
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

        /// <summary>
        /// Удалить из очереди экшен указанного владельца
        /// </summary>
        /// <param name="owner">Владелец запроса</param>
        public static void RemoveCall(object owner)
        {
            for (var i = _queue.Count - 1; i >= 0; i--)
                if (_queue[i].Owner == owner)
                    _queue.RemoveAt(i);
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
            if (_queue.Count == 0) return;

            // Удалены временные списки и пересоздание списков
            // Меньше нагрузка на GC

            ReadyQueue.Clear();

            // Идём с конца, удаляем сразу
            for (int i = _queue.Count - 1; i >= 0; i--)
            {
                //Запускаем актуальные, остальные набиваем в новый список 
                var actionData = _queue[i];
                if (actionData.Timestamp <= Time)
                {
                    ReadyQueue.Add(actionData.Action);
                    _queue.RemoveAt(i);
                }
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

        #endregion
    }
}