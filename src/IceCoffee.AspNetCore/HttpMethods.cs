using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.AspNetCore
{
    public struct HttpMethod
    {
        public const string Any = "*";

        public const string Get= "GET";
        public const string Post= "POST";
        public const string Head = "HEAD";

        public const string Put= "PUT";
        public const string Delete= "DELETE";
        public const string Patch = "PATCH";
        public const string Options = "OPTIONS";
    }
}
