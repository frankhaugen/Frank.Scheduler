using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Frank.Scheduler.Client.ServiceBus.Producers
{
    public class SchedulerResponseProducer
    {
        public async Task ProduceMessage(object message, string label)
        {
            var topicClient = new TopicClient("Endpoint", "topicname");

            var jsonMessage = JsonConvert.SerializeObject(message);
            var topicMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                ContentType = "application/json",
                TimeToLive = TimeSpan.FromHours(24),
                Label = label
            };
            await topicClient.SendAsync(topicMessage);
            await topicClient.CloseAsync();
        }
    }
}
