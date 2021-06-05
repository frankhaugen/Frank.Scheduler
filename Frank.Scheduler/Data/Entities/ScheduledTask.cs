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

    public enum State
    {
        Running,
        Executing,
        Paused,
        Error
    }
}
