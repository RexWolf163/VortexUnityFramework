using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vortex.Core.ComplexModelSystem;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;

namespace Vortex.Unity.ComplexModelSystem
{
    [Serializable]
    public class ComplexModelUnity<T> : ComplexModel<T> where T : class
    {
        [SerializeReference] protected Dictionary<string, T> SerializedIndex = new();

        private void BeforeSerialization()
        {
            SerializedIndex = Index.ToDictionary(
                kv => kv.Key.FullName,
                kv => kv.Value
            );
        }

        public override string Serialize()
        {
            BeforeSerialization();
            //TODO вынести драйвер для сериализатора
            var str = JsonUtility.ToJson(this);
            return str;
            /*
            var ar = new List<string>(Index.Count);
            foreach (var type in Index.Keys)
            {
                var str = Index[type].Serialize();
                ar.Add($"{type.FullName}|{str}");
            }

            var serialized = string.Join(',', ar);
            var bResult = Encoding.UTF8.GetBytes(serialized);
            return Convert.ToBase64String(bResult);
        */
        }

        /// <summary>
        /// Десериализует модель из массива байтов.
        /// </summary>
        public override void Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Log.Print(LogLevel.Error, $"Attempt to deserialize from null or empty data.", this);
                return;
            }

            Index = JsonUtility.FromJson<Dictionary<Type, T>>(data);
            //TODO
        }
    }
}