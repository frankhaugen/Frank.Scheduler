using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Frank.Scheduler.Client.ServiceBus.Extensions
{
    public static class MessageExtensions
    {
        public static T GetBodyAs<T>(this Message message)
        {
            var body = message.GetBodyAsString();
            return JsonConvert.DeserializeObject<T>(body);
        }
        public static string GetBodyAsString(this Message message)
        {
            return Encoding.UTF8.GetString(message.Body);
        }
    }
}
