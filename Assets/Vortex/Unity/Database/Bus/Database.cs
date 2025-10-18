using System.Collections.Generic;
using UnityEngine;
using Vortex.Database.Model.Record;

namespace Vortex.Database.Bus
{
    public partial class DatabaseExtUnity
    {
        public static partial T GetRecord<T>(string guid) where T : Record
        {
            if (Application.isPlaying)
            {
                Debug.LogError("[DatabaseController] Call Editor function in play mode");
                return null;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
                Instance.LoadDb();
#endif
            return Instance._GetRecord<T>(guid);
        }

        /// <summary>
        /// Получить все записи указанного типа из БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static partial List<T> GetRecords<T>() where T : Record
        {
            if (Application.isPlaying)
            {
                Debug.LogError("[DatabaseController] Call Editor function in play mode");
                return null;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
                Instance.LoadDb();
#endif
            return Instance._GetRecords<T>();
        }
    }
}