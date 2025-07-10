using LogNotifier.Librarys;
using Serilog.Core;
using Serilog.Events;

namespace LogNotifier.Library
{
    public class NotifierSink : ILogEventSink, IDisposable
    {
        private readonly ILogNotifier _notifier;
        public NotifierSink(ILogNotifier notifier) { _notifier = notifier; }
        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level < LogEventLevel.Error) return;
            var msg = logEvent.RenderMessage();
            _notifier.SendAsync(new LoggingError { Exception = logEvent.Exception?.ToString() ?? string.Empty, Timestamp = logEvent.Timestamp, Level = logEvent.Level.ToString(), message = msg }).ConfigureAwait(false);
        }
        public void Dispose() { }
    }
}
