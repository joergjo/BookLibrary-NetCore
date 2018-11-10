using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace BookLibrary.Common
{
    public class EpochDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                long ticks = (long) reader.Value;
                var epoch = NewEpoch();
                var date = epoch.AddMilliseconds(ticks);
                return date;
            }

            throw new JsonSerializationException(
                $"Unexpected token type: Got {reader.TokenType}, expected {JsonToken.Integer}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                var epoch = NewEpoch();
                var timeSpan = ((DateTime) value) - epoch;
                if (timeSpan.TotalSeconds >= 0)
                {
                    long ticks = (long) timeSpan.TotalMilliseconds;
                    writer.WriteValue(ticks);
                    return;
                }

                throw new JsonSerializationException(
                    $"Unexpected epoch time {timeSpan.Seconds}.");
            }

            throw new JsonSerializationException(
                $"Unexpected type {value?.GetType()}.");
        }

        private static DateTime NewEpoch() => new DateTime(1970, 1, 1);
    }
}
