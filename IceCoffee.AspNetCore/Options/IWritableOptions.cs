using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.AspNetCore.Options
{
    public interface IWritableOptions<TOptions> where TOptions : class, new()
    {
        TOptions CurrentValue { get; }

        TOptions Get(string name);

        void Update(TOptions options);
    }
}
