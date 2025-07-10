using LogNotifier.Librarys;
using Microsoft.Extensions.Configuration;

namespace LogNotifier.Library;
public class LogNotifierSelector : ILogNotifier
{
    private readonly DiscordLogNotifier _discord;
    private readonly TelegramLogNotifier _telegram;
    private readonly string _logChannel;

    public LogNotifierSelector(DiscordLogNotifier discord, TelegramLogNotifier telegram, IConfiguration config)
    {
        _discord = discord;
        _telegram = telegram;
        _logChannel = config["Logging:LogChannel"];
    }

    public async Task SendAsync(LoggingError logging)
    {
        if (_logChannel == "Telegram")
            await _telegram.SendAsync(logging);
        else
            await _discord.SendAsync(logging);
    }
}