
# LogNotifier.Library

## Configuration Guide

### 1. Configuration in `appsettings.json`

Add the following section to your `appsettings.json` file (you can configure either Telegram, Discord, or both):

```json
{
  "Logging": {
    "LogChannel": "Discord", // or "Telegram"
    "Discord": {
      "WebhookUrl": "https://discord.com/api/webhooks/xxxx/xxxx"
    },
    "Telegram": {
      "BotToken": "YOUR_TELEGRAM_BOT_TOKEN",
      "ChatId": "YOUR_TELEGRAM_CHAT_ID"
    }
  }
}
```

- `LogChannel`: Choose `"Discord"` or `"Telegram"` as the preferred channel for sending logs.
- `Discord:WebhookUrl`: Discord Webhook URL.
- `Telegram:BotToken`: Telegram bot token.
- `Telegram:ChatId`: Chat ID to receive logs.

---

### 2. Configuration in `Program.cs`

Add the following lines to your `Program.cs` (or wherever you configure Dependency Injection):

```csharp
using LogNotifier.Library;
using Serilog;

// ... other using statements

var builder = WebApplication.CreateBuilder(args);

// Register LogNotifier in DI
builder.Services.AddLogNotifiers();

// Configure Serilog to use LogNotifier
builder.Host.UseSerilog((context, services, configuration) =>
{
    var notifier = services.GetRequiredService<ILogNotifier>();
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Notifier(notifier); // Send error logs via LogNotifier
});

// ... other configurations

var app = builder.Build();

// ... middleware, endpoints, etc.

app.Run();
```

---

### 3. Notes

- This library only sends logs with **Error** level or higher.
- Make sure to install the required packages:
  - `Serilog`
  - `Microsoft.Extensions.DependencyInjection`
  - `Microsoft.Extensions.Http`
  - `Microsoft.Extensions.Caching.Memory`

---

### Full Example

**appsettings.json:**
```json
{
  "Logging": {
    "LogChannel": "Discord",
    "Discord": {
      "WebhookUrl": "https://discord.com/api/webhooks/xxxx/xxxx"
    }
  }
}
```

**Program.cs:**
```csharp
using LogNotifier.Library;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogNotifiers();
builder.Host.UseSerilog((context, services, configuration) =>
{
    var notifier = services.GetRequiredService<ILogNotifier>();
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Notifier(notifier);
});
var app = builder.Build();
app.Run();
```

---

If you need a more detailed example or run into issues during setup, feel free to provide more information for support!
