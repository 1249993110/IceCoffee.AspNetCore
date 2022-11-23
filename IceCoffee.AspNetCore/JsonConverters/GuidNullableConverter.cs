﻿using IceCoffee.Common.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.JsonConverters
{
    /// <summary>
    /// 将Guid序列化为大写字符串
    /// </summary>
    public class GuidNullableConverter : JsonConverter<Guid?>
    {
        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return Guid.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString().ToUpper());
        }
    }
}