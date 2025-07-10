using System.Text;
using System.Text.Json;
using LogNotifier.Librarys;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace LogNotifier.Library
{
    public class DiscordLogNotifier
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;
        private readonly string _webhookUrl;
        public DiscordLogNotifier(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, IMemoryCache cache, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _cache = cache;
            _webhookUrl = config["Logging:Discord:WebhookUrl"];
        }
        public async Task SendAsync(LoggingError logging)
        {
            if (_cache.TryGetValue("DiscordLogNotifier_" + logging.Exception + logging.Level, out LoggingError cachedLogging))
            {
                if (cachedLogging.message == logging.message)
                {
                    return; // Tránh gửi trùng lặp
                }
                else
                {
                    _cache.Set<LoggingError>("DiscordLogNotifier_" + logging.Exception + logging.Level, logging, TimeSpan.FromHours(1));
                }

            }
            else
            {
                // Thêm vào cache nếu không có
                _cache.Set("DiscordLogNotifier_" + logging.Exception + logging.Level, logging, TimeSpan.FromHours(1));
            }
            //var _cache = _serviceProvider.GetRequiredService<IDistributedCache<LoggingError>>();
            //var loggingCache  = await _cache.GetOrAddAsync("DiscordLogNotifier_"+logging.Exception+ logging.Level, async () =>
            //{
            //    var error = logging;
            //    return error;
            //},
            //    () => new DistributedCacheEntryOptions
            //    {
            //        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
            //    });
            //if (loggingCache != null && loggingCache.message == logging.message)
            //{
            //    return; // Tránh gửi trùng lặp
            //}
            var client = _httpClientFactory.CreateClient();
            if (logging.message.Length > 200)
            {
                logging.message = logging.message.Substring(logging.message.Length - 200);
            }
            var payload = new { content = $"🚨 **LỖI HỆ THỐNG** 🚨\n [{logging.Timestamp:O}] {logging.Level}: \n{logging.Exception} ]\": \n{logging.message}" };
            var json = JsonSerializer.Serialize(payload);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_webhookUrl, content);

            var contentrs = response.Content.ReadAsStringAsync();// Bắn exception nếu lỗi
        }
    }
}
