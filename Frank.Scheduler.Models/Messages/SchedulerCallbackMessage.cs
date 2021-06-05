using System;

namespace Frank.Scheduler.Models.Messages
{
    public class SchedulerCallbackMessage : SchedulerResult
    {
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public MessageException? Exception { get; set; }
    }
}
