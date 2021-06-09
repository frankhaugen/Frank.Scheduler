namespace Frank.Scheduler.Client.Configuration
{
    public class SchedulerServiceBusConfiguration
    {
        public string ServiceBusEndpoint { get; set; }
        public string ServiceBusTopicName { get; set; }
        public short LockDurationMinutes { get; set; }
        public string MicroServiceConsumerLabel { get; set; }
    }
}
