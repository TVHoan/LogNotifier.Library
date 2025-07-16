# 📦 LogNotifier.Library

A lightweight .NET library that sends error logs and critical events to Discord or Telegram using webhooks — without modifying your existing logging setup. Ideal for real-time monitoring and incident alerts in production environments.

---

## 🚀 Installation

You can install this package directly from [NuGet.org](https://www.nuget.org/packages/LogNotifier.Library):

### Using .NET CLI:

```bash
dotnet add package LogNotifier.Library
```

### Using Package Manager:

```powershell
Install-Package LogNotifier.Library
```

---

## ⚙️ Configuration (`appsettings.json`)

Add the following section to your `appsettings.json` file:

```json
{
  "Logging": {
    "LogChannel": "Discord", // or "Telegram"
    "Discord": {
      "WebhookUrl": "https://discord.com/api/webhooks/your_webhook_here"
    },
    "Telegram": {
      "BotToken": "your_bot_token",
      "ChatId": "your_chat_id"
    }
  }
}
```

---

## 🧪 Usage

### 1. Register the service in `Program.cs` (or `Startup.cs` depending on your .NET version):


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

### 2. Use `ILogger` to log messages:

```csharp
private readonly ILogger<HomeController> _logger;

public HomeController(ILogger<HomeController> logger)
{
    _logger = logger;
}

public IActionResult Index()
{
    _logger.LogError("A critical error occurred");
    return View();
}
```

---

## 📌 Notes

- You can configure it to send logs to both Discord and Telegram simultaneously.
- Fully supports ASP.NET Core and .NET Console applications.

---

## 📚 License

MIT License
