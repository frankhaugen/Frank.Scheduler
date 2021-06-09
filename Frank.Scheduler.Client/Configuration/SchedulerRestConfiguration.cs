using System;

namespace Frank.Scheduler.Client.Configuration
{
    public class SchedulerRestConfiguration
    {
        public Uri BaseUrl { get; set; }

        public string ApiVersion { get; set; }

        public bool ExtendedLogging { get; set; }

        public bool ForceInternalToken { get; set; }

        public int? Timeout { get; set; }
    }
}