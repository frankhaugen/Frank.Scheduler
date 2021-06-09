using System;

namespace Frank.Scheduler.Models.Messages
{
    public class SchedulerTriggerMessage
    {
        public SchedulerTriggerMessage(Guid id, string callbackLabel)
        {
            Id = id;
            CallbackLabel = callbackLabel;
        }

        public Guid Id { get; }
        public string CallbackLabel { get; }
    }

    public class SchedulerResult
    {
        public Result Result { get; set; }
        public string? Message { get; set; }
    }
}
