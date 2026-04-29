namespace IceCoffee.AspNetCore.Options
{
    /// <summary>
    /// Configuration options for the HTTP Basic Authentication scheme, bound from the
    /// <c>BasicAuthOptions</c> section of <c>appsettings.json</c>. When <see cref="Enabled"/> is
    /// <see langword="true"/>, the application registers Basic Auth as its primary authentication
    /// mechanism and exposes the corresponding Swagger security definition.
    /// </summary>
    public class BasicAuthOptions
    {
        /// <summary>
        /// The authentication realm sent in the <c>WWW-Authenticate</c> challenge header,
        /// displayed by browsers in the native credential prompt dialog.
        /// Defaults to <c>"Basic Authentication"</c>.
        /// </summary>
        public string Realm { get; set; } = "Basic Authentication";

        /// <summary>
        /// Controls whether HTTP Basic Authentication is activated for this deployment.
        /// When <see langword="false"/>, all authentication and Swagger security configuration
        /// related to Basic Auth is skipped entirely, which is the desired behaviour for
        /// deployments that rely on a different auth strategy.
        /// </summary>
        public bool Enabled { get; init; }

        /// <summary>
        /// The username accepted for Basic Authentication. Must be supplied via configuration
        /// and should never be hard-coded in source control.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The password accepted for Basic Authentication. Treat this as a secret and supply
        /// it through a secrets manager or environment variable rather than plain appsettings.
        /// </summary>
        public required string Password { get; set; }
    }
}
