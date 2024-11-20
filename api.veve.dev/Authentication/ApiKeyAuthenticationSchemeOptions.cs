using Microsoft.AspNetCore.Authentication;

namespace veve.Authentication
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string HeaderName { get; set; } = "X-Api-Key";
        public string? ApiKey { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(HeaderName))
            {
                throw new ArgumentException("Header name must be provided.");
            }

            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ArgumentException("API key must be provided.");
            }
        }
    }
}
