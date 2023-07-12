using IceCoffee.Common.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.JsonConverters
{
    public class DateTimeNullableConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return DateTime.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToStringWithoutT());
        }
    }
}