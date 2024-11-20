using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace veve.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    TimeProvider timeProvider,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? apiKeyHeader = configuration["Authentication:ApiKeyHeader"];
        string? apiKey = configuration["Authentication:ApiKey"];

        if (apiKeyHeader is null || apiKey is null)
            throw new Exception("Authentication configuration is invalid.");

        if (!Request.Headers.TryGetValue(apiKeyHeader, out var apiKeyHeaderValues))
            return Task.FromResult(AuthenticateResult.Fail("API Key was not provided."));

        string? clientApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (clientApiKey is null)
            return Task.FromResult(AuthenticateResult.Fail("API Key was not provided."));

        if (clientApiKey != apiKey)
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key provided."));

        var claims = new[] { new Claim(ClaimTypes.Name, "VALID_USER") };
        var identity = new ClaimsIdentity(claims, ApiKeyDefaults.AuthenticationScheme);
        var identities = new[] { identity };
        var principal = new ClaimsPrincipal(identities);
        var ticket = new AuthenticationTicket(principal, ApiKeyDefaults.AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public static class ApiKeyDefaults
{
    public static string AuthenticationScheme { get; set; } = "ApiKey";
}