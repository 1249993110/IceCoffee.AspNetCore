using System.Text.Json;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.JsonConverters
{
    /// <summary>
    /// A <see cref="System.Text.Json"/> converter that normalises <see cref="DateTime"/> serialisation
    /// to the <c>yyyy-MM-ddTHH:mm:ss.fffZ</c> format, ensuring consistent ISO 8601 timestamps across
    /// all API responses regardless of the server's local culture or timezone settings.
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Deserialises a JSON string token into a <see cref="DateTime"/> by delegating to
        /// <see cref="DateTime.Parse(string)"/>. Accepts any ISO 8601 representation sent by clients,
        /// keeping the inbound contract flexible while the outbound format remains strictly normalised.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> positioned at the string token.</param>
        /// <param name="typeToConvert">The target type (always <see cref="DateTime"/> for this converter).</param>
        /// <param name="options">The active <see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The parsed <see cref="DateTime"/> value.</returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()!);
        }

        /// <summary>
        /// Serialises a <see cref="DateTime"/> value as a JSON string using the fixed format
        /// <c>yyyy-MM-ddTHH:mm:ss.fffZ</c>, making all API response timestamps machine-parseable
        /// and timezone-unambiguous for downstream consumers.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write into.</param>
        /// <param name="value">The <see cref="DateTime"/> value to serialise.</param>
        /// <param name="options">The active <see cref="JsonSerializerOptions"/>.</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }
    }
}