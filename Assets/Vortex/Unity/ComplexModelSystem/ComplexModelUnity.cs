using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using UnityEngine;
using Vortex.Core.ComplexModelSystem;

namespace Vortex.Unity.ComplexModelSystem
{
    [ProtoContract]
    public class ComplexModelUnity<T> : ComplexModel<T> where T : class
    {
        [ProtoMember(1)] protected Dictionary<string, T> SerializedIndex = new();

        [ProtoBeforeSerialization]
        private void BeforeSerialization()
        {
            SerializedIndex = Index.ToDictionary(
                kv => kv.Key.AssemblyQualifiedName,
                kv => kv.Value
            );
        }

        /// <summary>
        /// Сериализует модель в массив байтов.
        /// </summary>
        public byte[] SerializeToBytes()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Десериализует модель из массива байтов.
        /// </summary>
        public void DeserializeFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}] Attempt to deserialize from null or empty byte array.");
                return;
            }

            using (var stream = new MemoryStream(data))
            {
                var result = Serializer.Deserialize<ComplexModelUnity<T>>(stream);
                Index.Clear();
                foreach (var (typeName, instance) in result.SerializedIndex)
                {
                    var type = Type.GetType(typeName);
                    if (type != null)
                        Index[type] = instance;
                    else
                        Debug.LogWarning($"[{GetType().Name}] Cannot resolve data type: «{typeName}»");
                }
            }
        }
    }
}