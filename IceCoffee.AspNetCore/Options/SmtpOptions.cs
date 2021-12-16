using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IceCoffee.AspNetCore.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; }

        public int Port { get; set; }

        [DefaultValue(true)]
        public bool EnableSsl { get; set; } = true;

        [DefaultValue(true)]
        public bool UseDefaultCredentials { get; set; } = true;

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
