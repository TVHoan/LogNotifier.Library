using LogNotifier.Librarys;
using Microsoft.Extensions.DependencyInjection;

namespace LogNotifier.Library
{
    public static class LogNotifierServiceCollectionExtensions
    {
        public static IServiceCollection AddLogNotifiers(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<DiscordLogNotifier>();
            services.AddSingleton<TelegramLogNotifier>();
            services.AddSingleton<ILogNotifier, LogNotifierSelector>();
            services.AddMemoryCache();
            return services;
        }
    }
}
