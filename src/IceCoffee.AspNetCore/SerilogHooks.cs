using Serilog.Sinks.File.Archive;
using System.IO.Compression;

namespace IceCoffee.AspNetCore
{
    /// <summary>
    /// Provides pre-configured hook instances for the Serilog logging system,
    /// designed to be attached to file sinks to automate log archival in production environments.
    /// </summary>
    public class SerilogHooks
    {
        /// <summary>
        /// A pre-configured <see cref="ArchiveHooks"/> instance that automatically compresses and
        /// archives rolled log files using the smallest compression ratio, retaining archives for
        /// 180 days. Attach this to a Serilog file sink to keep disk usage bounded in
        /// long-running production services.
        /// </summary>
        public static ArchiveHooks ArchiveHooks => new ArchiveHooks(180, CompressionLevel.SmallestSize);
    }
}
