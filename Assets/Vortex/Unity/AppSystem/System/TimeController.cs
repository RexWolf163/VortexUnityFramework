using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.AppSystem.System
{
    /// <summary>
    /// Класс таймера.
    /// Центральный диспетчер для вызова экшенов по времени
    /// </summary>
    public class TimeController : MonoBehaviour
    {
        #region InnerClasses

        /// <summary>
        /// Структура экшена в очереди
        /// </summary>
        private class QueuedAction
        {
            /// <summary>
            /// Заказчик события
            /// </summary>
            [FoldoutGroup("$Timer")] public object owner;

            /// <summary>
            /// Отложенный экшен 
            /// </summary>
            [FoldoutGroup("$Timer")] public Action action;

            /// <summary>
            /// Отметка времени, в которую должен быть вызван
            /// </summary>
            [FoldoutGroup("$Timer")] public double timestamp;

            public void Set(Action action, double timestamp)
            {
                this.action = action;
                this.timestamp = timestamp;
            }

#if UNITY_EDITOR
            private string Timer()
            {
                var span = new TimeSpan((long)((timestamp - Time) * TicksPerSecond));
                return span.ToString(@"hh\:mm\:ss");
            }
#endif
        }

        #endregion

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
        private const double TicksPerSecond = 10000000;

        /// <summary>
        /// Шаг проверки очереди экшенов.
        /// Чтобы разгрузить проц от проверки на каждом кадре
        /// </summary>
        private const float stepTime = 0.1f;

        /// <summary>
        /// Отметка времени последней проверки очереди 
        /// </summary>
        private static double lastCheckTime = -1;

        // Переиспользуемый буффер, избавляемся от пересоздания списков
        private static readonly List<QueuedAction> readyQueue = new();

        /// <summary>
        /// Очередь на срабатывание
        /// </summary>
        [ShowInInspector, HideInEditorMode] private static List<QueuedAction> queue = new();

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
            if (action == null)
                return;

            if (stepSecs <= 0f)
                lastCheckTime = Time - stepTime;

            var triggerTime = Time + stepSecs;

            if (owner == null)
            {
                // удалено .Clone()
                // делегаты в C# неизменяемы, Clone() создаёт лишние вызовы

                queue.Add(new QueuedAction
                {
                    owner = null,
                    action = action,
                    timestamp = triggerTime
                });
                return;
            }

            foreach (var queuedAction in queue)
            {
                if (queuedAction.owner != owner)
                    continue;

                queuedAction.Set(action, triggerTime);
                return;
            }

            queue.Add(new QueuedAction
            {
                owner = owner,
                action = action,
                timestamp = triggerTime
            });
        }


        /// <summary>
        /// Удалить из очереди экшен указанного владельца
        /// </summary>
        /// <param name="owner">Владелец запроса</param>
        public static void RemoveCall(object owner)
        {
            for (var i = 0; i < queue.Count; i++)
                if (queue[i].owner == owner)
                    queue.RemoveAt(i);
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

        private void Awake()
        {
            DontDestroyOnLoad(this);
            isInit = true;
        }

        /// <summary>
        /// Проверка очереди запросов и активация тех, чье время пришло
        /// </summary>
        private void CheckQueue()
        {
            if (queue.Count == 0) return;

            // Удалены временные списки и пересоздание списков
            // Меньше нагрузка на GC

            readyQueue.Clear();

            // Идём с конца, удаляем сразу
            for (int i = queue.Count - 1; i >= 0; i--)
            {
                //Запускаем актуальные, остальные набиваем в новый список 
                var actionData = queue[i];
                if (actionData.timestamp <= Time)
                {
                    readyQueue.Add(actionData);
                    queue.RemoveAt(i);
                }
            }

            // восстанавливаем оригинальный порядок
            readyQueue.Reverse();

            foreach (var actionData in readyQueue)
            {
                try
                {
                    actionData.action?.Invoke();
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
            if (Time - lastCheckTime < stepTime)
                return;
            lastCheckTime = Time;
            CheckQueue();
        }

        #endregion

        private static bool isInit;

        public static bool IsInit()
        {
            return isInit;
        }
    }
}