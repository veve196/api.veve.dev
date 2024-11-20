using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace veve.Authentication
{
    public class ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TimeProvider timeProvider,
        IConfiguration configuration)
        : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
#if !DEBUG
            if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
                return AuthenticateResult.Fail("API Key was not provided.");

            if (apiKeyHeaderValues.Count > 1)
                return AuthenticateResult.Fail("Multiple API keys found in request. Please only provide one key.");

            if (string.IsNullOrEmpty(Options.ApiKey) || !Options.ApiKey.Equals(apiKeyHeaderValues.FirstOrDefault()))
                return AuthenticateResult.Fail("Invalid API key.");
#endif
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, Options.ApiKey),
                new Claim(ClaimTypes.Name, "API Key User"),
                new Claim(ClaimTypes.Role, "ReadOnly")
            ];

            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}