namespace veve.Models.Discord
{
    public class SpotifyActivity
    {
        public required string Title { get; set; } // RichPresence.Details
        public required string Artists { get; set; } // RichPresence.State
        public required string Album { get; set; } // RichPresence.LargeImageText
        public required Uri CoverUrl { get; set; } // RichPresence.LargeImage.Url
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
