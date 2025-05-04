using DSharpPlus;
using DSharpPlus.Entities;
using veve.Models.Discord;

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

        public async Task<User> GetUserAsnyc(ulong userId)
        {
            DiscordUser? user;

            try
            {
                user = await Client.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get Discord user with id {userId}. Is the Id correct?", ex);
            }

            return new User
            {
                Id = user.Id,
                AvatarUrl = user.AvatarUrl,
                Status = user.Presence.Status.ToString().ToLower()
            };
        }

        public async Task<SpotifyActivity?> GetSpotifyActivityAsync(ulong userId)
        {
            DiscordUser? user;

            try
            {
                user = await Client.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get Discord user with id {userId}. Is the Id correct?", ex);
            }

            DiscordActivity? spotifyActivity = user.Presence.Activities
                                                .FirstOrDefault(a => a.ActivityType == ActivityType.ListeningTo &&
                                                                a.Name == "Spotify");

            if (spotifyActivity is null)
                return null;

            return new SpotifyActivity
            {
                Title = spotifyActivity.RichPresence.Details,
                Artist = spotifyActivity.RichPresence.State,
                Album = spotifyActivity.RichPresence.LargeImageText,
                CoverUrl = spotifyActivity.RichPresence.LargeImage.Url,
                StartDate = spotifyActivity.RichPresence.StartTimestamp?.ToUniversalTime(),
                EndDate = spotifyActivity.RichPresence.EndTimestamp?.ToUniversalTime()
            };
        }
    }
}
