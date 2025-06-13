using JasperFx.Core;
using Wolverine;
using Wolverine_Rabbit.Metrics;

namespace Wolverine_Rabbit.HostedServices;

public class PingerService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var count = 0;

        await using var scope  = serviceProvider.CreateAsyncScope();
        var             bus    = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        var             logger = scope.ServiceProvider.GetRequiredService<ILogger<PingerService>>();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = new PingMessage
                {
                    Number = ++count
                };

                await bus.SendAsync(message);
                MetricsCatalog.CountMessagesSent.Add(1);
                logger.LogInformation("Sent PingMessage: {PingMessageNumber}", message.Number);
                await Task.Delay(1.Seconds(), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }
}