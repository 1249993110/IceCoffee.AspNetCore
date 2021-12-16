using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IceCoffee.AspNetCore.Options
{
    public class WritableOptions<TOptions> : IWritableOptions<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsMonitor<TOptions> _options;
        private readonly string _propertyName;
        private readonly string _path;

        public WritableOptions(
            IOptionsMonitor<TOptions> options,
            string propertyName,
            string path)
        {
            _options = options;
            _propertyName = propertyName;
            _path = path;
        }

        public TOptions CurrentValue => _options.CurrentValue;
        public TOptions Get(string name) => _options.Get(name);

        public void Update(TOptions options)
        {
            string json = File.ReadAllText(_path, Encoding.UTF8);

            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            jObject[_propertyName] = JObject.Parse(JsonConvert.SerializeObject(options));

            File.WriteAllText(_path, JsonConvert.SerializeObject(jObject, Formatting.Indented), Encoding.UTF8);
        }
    }
}
