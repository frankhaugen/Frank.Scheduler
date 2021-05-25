using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frank.Scheduler.Api.ServiceBus
{
    public class ServiceBusMessageConsumer : BackgroundService
    {
        private readonly ILogger<ServiceBusMessageConsumer> _logger;
        private readonly IServiceBusSubscriptionClientFactory _clientFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ServiceBusConfiguration _options;

        private ISubscriptionClient _subscriptionClient;

        public ServiceBusMessageConsumer(ILogger<ServiceBusMessageConsumer> logger, IServiceBusSubscriptionClientFactory serviceBusSubscriptionClientFactory, IOptions<ServiceBusConfiguration> options, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _clientFactory = serviceBusSubscriptionClientFactory;
            _scopeFactory = scopeFactory;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriptionClient = await _clientFactory.CreateSubscriptionClientAsync(_options.Endpoint, _options.TopicName, _options.SubscriptionName, _options.Filter, _options.LockDurationMinutes);
            _subscriptionClient.RegisterMessageHandler(ConsumeMessage, new MessageHandlerOptions(LogMessageHandlerException) { AutoComplete = true, MaxConcurrentCalls = 1 });
        }

        private Task LogMessageHandlerException(ExceptionReceivedEventArgs e)
        {
            _logger.LogCritical(e.Exception, e.Exception.Message);
            return Task.CompletedTask;
        }

        private async Task ConsumeMessage(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTimeOffset.UtcNow}: Message received!");

            var body = Encoding.UTF8.GetString(message.Body);
            _logger.LogInformation(body);

            using var scope = _scopeFactory.CreateScope();

            try
            {

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    _logger.LogCritical(e.InnerException, e.InnerException.Message);
                else
                    _logger.LogCritical(e, e.Message);
            }
        }
    }
}
