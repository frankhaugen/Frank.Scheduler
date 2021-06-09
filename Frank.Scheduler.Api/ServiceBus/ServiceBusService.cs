using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frank.Scheduler.Api.ServiceBus
{
    public class ServiceBusService : IServiceBusService
    {
        private readonly ServiceBusConfiguration _options;

        public ServiceBusService(IOptions<ServiceBusConfiguration> options)
        {
            _options = options.Value;
        }

        public async Task<Message> SendMessage<T>(Guid messageId, string messageLabel, T body) =>
            await SendMessage(new Message(JsonSerializer.SerializeToUtf8Bytes(body))
            {
                Label = messageLabel,
                ContentType = "application/json",
                MessageId = messageId.ToString(),
                TimeToLive = TimeSpan.FromHours(1)
            });

        public async Task<Message> SendMessage(Guid messageId, string messageLabel, string messageBody) =>
            await SendMessage(new Message(Encoding.UTF8.GetBytes(messageBody))
            {
                Label = messageLabel,
                ContentType = "application/json",
                MessageId = messageId.ToString(),
                TimeToLive = TimeSpan.FromHours(1)
            });

        private async Task<Message> SendMessage(Message message)
        {
            var topicClient = new TopicClient(_options.Endpoint, _options.TopicName, RetryPolicy.Default);
            await topicClient.SendAsync(message);

            return message;
        }
    }
}
