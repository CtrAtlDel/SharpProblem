using Confluent.Kafka;
using Google.Protobuf;

namespace Calculation.ProtoHelpers
{
    public class ProtoSerializer<T> : ISerializer<T> 
        where T : IMessage<T>
    {
        public byte[] Serialize(T data, SerializationContext context) =>
            data.ToByteArray();
    }
}