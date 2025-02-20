namespace veve.Models.Discord
{
    public class User
    {
        public required ulong Id { get; set; }
        public required string AvatarUrl { get; set; }
        public required string Status { get; set; }
    }
}
