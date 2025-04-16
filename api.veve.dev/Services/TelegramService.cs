using SpotifyAPI.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace veve.Services
{
    public class TelegramService(ILogger<TelegramService> logger, IConfiguration config, ITelegramBotClient botClient)
    {
        public async Task<string[]> GetStickerFileIds(string stickerSetName)
        {
            List<string> fileIds = [];
            StickerSet stickerSet;

            try
            {
                stickerSet = await botClient.GetStickerSet(stickerSetName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get sticker set with name {stickerSetName}. Is the name correct?", ex);
            }

            foreach (Sticker s in stickerSet.Stickers)
                fileIds.Add(s.FileId);

            return [.. fileIds];
        }

        public async Task<string> GetStickerUrl(string fileId)
        {
            try
            {
                TGFile file = await botClient.GetFile(fileId);
                return $"https://api.telegram.org/file/bot{config.GetValue<string>("Telegram:Token")}/{file.FilePath}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get sticker with file id {fileId}. Is the file id correct?", ex);
            }
        }
    }
}
