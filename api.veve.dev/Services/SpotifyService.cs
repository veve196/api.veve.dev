using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;

namespace veve.Services
{
    public class SpotifyService
    {
        protected readonly SpotifyClient _client;
        protected readonly IConfiguration _configuration;

        public SpotifyService(IConfiguration configuration)
        {
            _configuration = configuration;

            string clientId = configuration.GetValue<string>("Spotify:ClientId") ??
                                throw new Exception("Client ID missing.");
            string clientSecret = configuration.GetValue<string>("Spotify:ClientSecret") ??
                                throw new Exception("Client Secret missing.");

            var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(clientId, clientSecret));
            
            _client = new(config);
        }
    }
}
