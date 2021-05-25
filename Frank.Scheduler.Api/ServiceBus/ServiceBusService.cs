using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;

namespace Frank.Scheduler.Api.ServiceBus
{
    public class ServiceBusService : IServiceBusService
    {
        private readonly ServiceBusConfiguration _options;

        public ServiceBusService(IOptions<ServiceBusConfiguration> options)
        {
            _options = options.Value;
        }

        public async Task SendMessage(string message) => await SendMessage(Encoding.UTF8.GetBytes(message));

        private async Task SendMessage(byte[] body)
        {
            var message = new Message
            {
                Body = body,
                ContentType = "application/json",
                TimeToLive = TimeSpan.FromHours(24),
                Label = _options.Filter
            };

            var topicClient = new TopicClient(_options.Endpoint, _options.TopicName, RetryPolicy.Default);
            await topicClient.SendAsync(message);
        }
    }
}
