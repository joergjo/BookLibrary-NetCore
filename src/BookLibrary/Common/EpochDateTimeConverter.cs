using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookLibrary.Common
{
    public class EpochDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetInt64(out long ticks))
            {
                var epoch = NewEpoch();
                var date = epoch.AddMilliseconds(ticks);
                return date;
            }
            else
            {
                return default;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                var epoch = NewEpoch();
                var timeSpan = value.Value - epoch;
                long ticks = (long)timeSpan.TotalMilliseconds;
                writer.WriteNumberValue(ticks);
            }
            else
            {
                writer.WriteNullValue();
            }
        }

        protected static DateTime NewEpoch() => new DateTime(1970, 1, 1);
    }
}
