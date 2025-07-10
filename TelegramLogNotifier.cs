using System.Text;
using System.Text.Json;
using LogNotifier.Librarys;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace LogNotifier.Library
{
    public class TelegramLogNotifier : ILogNotifier
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly string _botToken;
        private readonly string _chatId;

        public TelegramLogNotifier(IHttpClientFactory httpClientFactory, IMemoryCache cache, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _botToken = config["Logging:Telegram:BotToken"];
            _chatId = config["Logging:Telegram:ChatId"];
        }

        public async Task SendAsync(LoggingError logging)
        {
            // Chống gửi trùng lặp như DiscordLogNotifier
            if (_cache.TryGetValue("TelegramLogNotifier_" + logging.Exception + logging.Level, out LoggingError cachedLogging))
            {
                if (cachedLogging.message == logging.message) return;
                else _cache.Set("TelegramLogNotifier_" + logging.Exception + logging.Level, logging, TimeSpan.FromHours(1));
            }
            else
            {
                _cache.Set("TelegramLogNotifier_" + logging.Exception + logging.Level, logging, TimeSpan.FromHours(1));
            }

            var client = _httpClientFactory.CreateClient();
            var text = $"🚨 *LỖI HỆ THỐNG* 🚨\n[{logging.Timestamp:O}] {logging.Level}:\n{logging.Exception}\n{logging.message}";
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
            var payload = new { chat_id = _chatId, text = text, parse_mode = "Markdown" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync(url, content);
        }

  
    }
}