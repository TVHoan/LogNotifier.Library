# LogNotifier.Library

## Hướng dẫn cấu hình

### 1. Cấu hình trong `appsettings.json`

Thêm vào file `appsettings.json` của bạn đoạn cấu hình sau (tùy chọn Telegram hoặc Discord, hoặc cả hai):

```json
{
  "Logging": {
    "LogChannel": "Discord", // hoặc "Telegram"
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

- `LogChannel`: Chọn `"Discord"` hoặc `"Telegram"` để ưu tiên kênh gửi log.
- `Discord:WebhookUrl`: Webhook URL của Discord.
- `Telegram:BotToken`: Token bot Telegram.
- `Telegram:ChatId`: Chat ID nhận log.

---

### 2. Cấu hình trong `Program.cs`

Thêm các dòng sau vào `Program.cs` (hoặc nơi bạn cấu hình DI):

```csharp
using LogNotifier.Library;
using Serilog;

// ... các using khác

var builder = WebApplication.CreateBuilder(args);

// Thêm cấu hình LogNotifier vào DI
builder.Services.AddLogNotifiers();

// Cấu hình Serilog sử dụng LogNotifier
builder.Host.UseSerilog((context, services, configuration) =>
{
    var notifier = services.GetRequiredService<ILogNotifier>();
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Notifier(notifier); // Gửi log lỗi qua LogNotifier
});

// ... các cấu hình khác

var app = builder.Build();

// ... các middleware, endpoint, v.v.

app.Run();
```

---

### 3. Lưu ý

- Thư viện này chỉ gửi log có cấp độ **Error** trở lên.
- Đảm bảo đã cài đặt các package cần thiết: `Serilog`, `Microsoft.Extensions.DependencyInjection`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Caching.Memory`.

---

### Ví dụ tổng thể

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

Nếu bạn cần ví dụ cụ thể hơn hoặc gặp lỗi khi cấu hình, hãy cung cấp chi tiết để mình hỗ trợ thêm! 
