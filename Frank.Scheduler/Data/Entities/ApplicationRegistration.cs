using System;

namespace Frank.Scheduler.Data.Entities
{
    public class ApplicationRegistration
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MessageLabel { get; set; }
        public DateTime LastRegistration { get; set; }
    }
}
