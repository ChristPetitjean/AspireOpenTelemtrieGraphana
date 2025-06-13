using Wolverine;
using Wolverine.RabbitMQ;
using Wolverine.RabbitMQ.Internal;

namespace Wolverine_Rabbit;

internal static class WolverineExtension
{
    public static WebApplicationBuilder AddCustomWolverine(this WebApplicationBuilder builder)
    {
        builder.UseWolverine(opts =>
        {
            opts.ApplicationAssembly = typeof(Program).Assembly;

            // Listen for messages coming into the pongs queue
            opts.ListenToRabbitQueue("installation.activation");

            // Publish messages to the pings queue
            opts.PublishMessage<PingMessage>().ToRabbitExchange("installation.activation");

            opts.UseRabbitMqUsingNamedConnection("rabbit")
                .DeclareQueue("installation.activation",
                              cfg =>
                              {
                                  cfg.IsDurable   = true;
                                  cfg.AutoDelete  = false;
                                  cfg.QueueType   = QueueType.stream;
                                  cfg.IsExclusive = false;
                              })
                .DeclareExchange("installation.activation", exchange =>
                 {
                     // Also declares the queue too
                     exchange.BindQueue("installation.activation");
                 })
                .AutoProvision()
                .UseStreamsAsQueues();
        });

        return builder;
    }
}