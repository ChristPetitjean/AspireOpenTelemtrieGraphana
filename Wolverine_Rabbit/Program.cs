using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Wolverine_Rabbit;
using Wolverine;
using Wolverine.RabbitMQ;
using Wolverine.RabbitMQ.Internal;

// Custom metrics for the application
var greeterMeter   = new Meter("OtPrGrYa.Example", "1.0.0");
var countGreetings = greeterMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");

// Custom ActivitySource for the application
var greeterActivitySource = new ActivitySource("OtPrGrJa.Example");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
       .AddWolverine()
       .Services
       .AddOpenApi()
       .AddHostedService<PingerService>();

var app = builder.Build();

app.MapGet("/", SendGreeting);

app.Run();

string SendGreeting([FromServices]ILogger<Program> logger)
{
    // Create a new Activity scoped to the method
    using var activity = greeterActivitySource.StartActivity("GreeterActivity");

    // Log a message
    logger.LogInformation("Sending greeting");

    // Increment the custom counter
    countGreetings.Add(1);

    // Add a tag to the Activity
    activity?.SetTag("greeting", "Hello World!");

    return "Hello World!";
}

internal static class Extensions
{
    public static WebApplicationBuilder AddWolverine(this WebApplicationBuilder builder)
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
