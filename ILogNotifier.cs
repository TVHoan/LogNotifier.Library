namespace LogNotifier.Librarys
{
    public interface ILogNotifier
    {
        Task SendAsync(LoggingError logging);
    }
}
