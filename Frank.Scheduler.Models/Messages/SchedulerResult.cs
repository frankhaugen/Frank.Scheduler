using System;

namespace Frank.Scheduler.Models.Messages
{
    public class SchedulerTriggerMessage
    {
        public Guid Id { get; set; }
    }

    public class SchedulerResult
    {
        public Result Result { get; set; }
        public string? Message { get; set; }
    }
}
