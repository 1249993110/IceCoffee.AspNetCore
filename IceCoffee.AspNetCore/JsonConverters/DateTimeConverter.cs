using IceCoffee.Common.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.JsonConverters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                throw new FormatException("Can not convert the specified string to DateTime");
            }

            return DateTime.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToStringWithoutT());
        }
    }
}