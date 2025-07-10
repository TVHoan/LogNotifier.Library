
using LogNotifier.Library;
using LogNotifier.Librarys;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog;

//
// Summary:
//     Extends Serilog.LoggerConfiguration with methods for configuring asynchronous
//     logging.
public static class LoggerConfigurationAsyncExtensions
{
    //
    // Summary:
    //     Configure a sink to be invoked asynchronously, on a background worker thread.
    //     Accepts a reference to a monitor that will be supplied the internal state interface
    //     for health monitoring purposes.
    //
    // Parameters:
    //   loggerSinkConfiguration:
    //     The Serilog.Configuration.LoggerSinkConfiguration being configured.
    //
    //   configure:
    //     An action that configures the wrapped sink.
    //
    //   bufferSize:
    //     The size of the concurrent queue used to feed the background worker thread. If
    //     the thread is unable to process events quickly enough and the queue is filled,
    //     depending on blockWhenFull the queue will block or subsequent events will be
    //     dropped until room is made in the queue.
    //
    //   blockWhenFull:
    //     Block when the queue is full, instead of dropping events.
    //
    //   monitor:
    //     Monitor to supply buffer information to.
    //
    // Returns:
    //     A Serilog.LoggerConfiguration allowing configuration to continue.
    public static LoggerConfiguration Notifier(this LoggerSinkConfiguration loggerSinkConfiguration, ILogNotifier notifier)
    {
        if (loggerSinkConfiguration == null) throw new ArgumentNullException(nameof(loggerSinkConfiguration));
        if (notifier == null) throw new ArgumentNullException(nameof(notifier));
        return loggerSinkConfiguration.Sink(new NotifierSink(notifier), LogEventLevel.Error);
    }
}