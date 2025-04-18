using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace veve.Authentication
{
    public class ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
#if !DEBUG
            if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
                return Task.FromResult(AuthenticateResult.Fail("API Key was not provided."));

            if (apiKeyHeaderValues.Count > 1)
                return Task.FromResult(AuthenticateResult.Fail("Multiple API keys found in request. Please only provide one key."));

            if (string.IsNullOrEmpty(Options.ApiKey) || !Options.ApiKey.Equals(apiKeyHeaderValues.FirstOrDefault()))
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
#endif

            if (Options.ApiKey is null)
                return Task.FromResult(AuthenticateResult.Fail("API key is not configured."));

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, Options.ApiKey),
                new Claim(ClaimTypes.Name, "API Key User"),
                new Claim(ClaimTypes.Role, "ReadOnly")
            ];

            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}