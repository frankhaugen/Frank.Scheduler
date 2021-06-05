namespace Frank.Scheduler.Models.Messages
{
    public class MessageException
    {
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Site { get; set; }
    }
}
