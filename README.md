# 📦 LogNotifier.Library

A .NET library that helps you send logs to messaging channels like **Discord** or **Telegram** via webhook. It's designed for real-time monitoring of system errors and important events.

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
builder.Services.AddLogNotifier(builder.Configuration);
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
