namespace LogNotifier.Librarys
{
    public class LoggingError
    {
        public string message { get; set; }
        public string Exception { get; set; }
        public string Level { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
