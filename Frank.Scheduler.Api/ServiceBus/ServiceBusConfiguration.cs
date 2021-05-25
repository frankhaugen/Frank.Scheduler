namespace Frank.Scheduler.Api.ServiceBus
{
    public class ServiceBusConfiguration
    {
        public ServiceBusConfiguration()
        {

            string connectionString = "Endpoint=sb://dev-bilagos-bus.servicebus.windows.net/;SharedAccessKeyName=frank;SharedAccessKey=pz1WRdMWY1pjZVaCd6kyFACwogYeV+iyhuVyR63U/Yk=;";
            string topicName = "franklocal";
            string subscriptionName = "deleteme";
        }
        public string Endpoint { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
        public short LockDurationMinutes { get; set; }
        public string Filter { get; set; }
    }
}
