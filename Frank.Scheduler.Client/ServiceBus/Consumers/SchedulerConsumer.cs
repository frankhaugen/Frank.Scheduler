using Frank.Scheduler.Client.Configuration;
using Frank.Scheduler.Client.ServiceBus.Extensions;
using Frank.Scheduler.Client.ServiceBus.Interfaces;
using Frank.Scheduler.Client.ServiceBus.Producers;
using Frank.Scheduler.Models.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Frank.Scheduler.Client.ServiceBus.Consumers
{
    public class SchedulerConsumer : BackgroundService
    {
        private readonly ILogger<SchedulerConsumer> _logger;
        private readonly IServiceBusSubscriptionClientFactory _clientFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SchedulerServiceBusConfiguration _options;
        private readonly ISchedulerResponseProducer _responseProducer;

        private ISubscriptionClient _subscriptionClient;

        public SchedulerConsumer(ILogger<SchedulerConsumer> logger, IServiceBusSubscriptionClientFactory serviceBusSubscriptionClientFactory, IOptions<SchedulerServiceBusConfiguration> options, IServiceScopeFactory scopeFactory, ISchedulerResponseProducer responseProducer)
        {
            _logger = logger;
            _clientFactory = serviceBusSubscriptionClientFactory;
            _scopeFactory = scopeFactory;
            _responseProducer = responseProducer;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriptionClient = await _clientFactory.CreateSubscriptionClientAsync(_options.ServiceBusEndpoint, _options.ServiceBusTopicName, AppDomain.CurrentDomain.FriendlyName, _options.MicroServiceConsumerLabel, _options.LockDurationMinutes);
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
            var response = new SchedulerCallbackMessage()
            {
                Started = DateTime.UtcNow
            };

            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISchedulerHandler>();
            var trigger = message.GetBodyAs<SchedulerTriggerMessage>();

            try
            {
                var result = await service.TriggerScheduledTask(trigger.Id);
                response.Message = result.Message;
                response.Result = result.Result;
            }
            catch (Exception e)
            {
                var exception = new MessageException();

                if (e.InnerException != null)
                {
                    _logger.LogCritical(e.InnerException, e.InnerException.Message);
                    exception.Message = e.InnerException.Message;
                    exception.StackTrace = e.InnerException.StackTrace;
                    exception.Site = e.InnerException.TargetSite?.Name;
                }
                else
                {
                    _logger.LogCritical(e, e.Message);
                    exception.Message = e.Message;
                    exception.StackTrace = e.StackTrace;
                    exception.Site = e.TargetSite?.Name;
                }

                response.Exception = exception;
                response.Result = Result.Failure;
            }

            response.Finished = DateTime.UtcNow;

            await _responseProducer.ProduceMessage(response, trigger.CallbackLabel);

            await Task.CompletedTask;
        }
    }
}
