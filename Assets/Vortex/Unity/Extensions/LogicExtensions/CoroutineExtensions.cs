using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Vortex.Extensions
{
    /// <summary>
    /// Класс расширений для корутин
    /// </summary>
    public static class CoroutineExtensions
    {
        /// <summary>
        /// Представляет корутину как Task
        /// </summary>
        /// <param name="coroutine">Корутина</param>
        /// <param name="owner">MonoBehaviour запускающий корутину</param>
        /// <typeparam name="T">Возвращаемый тип данных</typeparam>
        /// <returns></returns>
        public static Task<T> AsTask<T>(this IEnumerator coroutine, MonoBehaviour owner)
        {
            var tcs = new TaskCompletionSource<T>();
            owner.StartCoroutine(RunCoroutine<T>(coroutine, tcs));
            return tcs.Task;
        }

        /// <summary>
        /// Запускает корутину
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="tcs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerator RunCoroutine<T>(IEnumerator coroutine, TaskCompletionSource<T> tcs)
        {
            while (coroutine.MoveNext())
            {
                if (coroutine.Current is T currentResult)
                {
                    tcs.SetResult(currentResult);
                }

                yield return coroutine.Current;
            }

            if (tcs.Task.IsCompleted) yield break;

            Debug.Log($"Unable to return T object in Coroutine as Task");
            tcs.SetResult(default);
        }
    }
}