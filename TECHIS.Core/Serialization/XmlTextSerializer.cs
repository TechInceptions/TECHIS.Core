

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TECHIS.Core.Serialization
{
    public class XmlTextSerializer : ITextSerializer
    {
        private ConcurrentDictionary<string, System.Xml.Serialization.XmlSerializer> _Serializers = new ConcurrentDictionary<string, System.Xml.Serialization.XmlSerializer>();
        public T Deserialize<T>(Stream ms)
        {
            Type type = typeof(T);
            var xs = _Serializers.GetOrAdd(type.FullName, (k) => new System.Xml.Serialization.XmlSerializer(type));
            if (ms.Length == 0)
            {
                return default;
            }
            else
            {
                ms.Position = 0;
                try
                {
                    //Deserialize without a reader.
                    return (T)(xs.Deserialize(ms));
                }
                catch
                {
                    //Deserialize with a reader.
                    ms.Position = 0;
                    return (T)DeserializeTextStream(ms, xs);
                }
            }
        }

        public Task<T> DeserializeAsync<T>(Stream stream)
        {
            return Task.FromResult<T>(Deserialize<T>(stream));
        }

        public void Serialize<T>(T obj, Stream ms)
        {
            Type type = typeof(T);
            var xs = _Serializers.GetOrAdd(type.FullName, (k) => new System.Xml.Serialization.XmlSerializer(type));
            xs.Serialize(ms, obj);
        }

        public Task SerializeAsync<T>(T obj, Stream stream)
        {
            Serialize<T>(obj, stream);
            return Task.CompletedTask;
        }

        private static object DeserializeTextStream(Stream ms, System.Xml.Serialization.XmlSerializer xs)
        {
            using (System.IO.StreamReader sr = new StreamReader(ms))
            {
                return xs.Deserialize(sr);
            }
        }
    }
}

