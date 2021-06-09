using System;

namespace Frank.Scheduler.Data.Entities
{
    public class ScheduledTask
    {
        public Guid Id { get; set; }
        public string Cron { get; set; }
        public DateTime? LastRun { get; set; }
        public State State { get; set; }
        public string Label { get; set; }


    }

    public class ScheduledTaskLog
    {
        public Guid Id { get; set; }

        public Guid? ScheduledTaskId { get; set; }
        public ScheduledTask? ScheduledTask { get; set; }
    }

    public class MessagesInProcessing
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }

        public string Label { get; set; }


        public Guid? ScheduledTaskId { get; set; }
        public ScheduledTask? ScheduledTask { get; set; }
    }
}
