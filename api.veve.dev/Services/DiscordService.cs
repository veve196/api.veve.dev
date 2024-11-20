using DSharpPlus;

namespace veve.Services
{
    public class DiscordService
    {
        readonly IConfiguration _config;
        public DiscordClient Client { get; private set; }

        public DiscordService(IConfiguration config)
        {
            _config = config;

            DiscordConfiguration discordConfig = new()
            {
                Token = _config["Discord:Token"],
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildPresences
            };

            Client = new(discordConfig);
        }
    }
}
