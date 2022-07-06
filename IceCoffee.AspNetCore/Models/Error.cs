using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IceCoffee.AspNetCore.Models
{
    public class Error
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("details")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Details { get; set; }
    }
}
