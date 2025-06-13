using Microsoft.AspNetCore.Mvc;
using Wolverine_Rabbit.Metrics;

namespace Wolverine_Rabbit.Endpoints
{
    public static class GreetingEndpoints
    {
        public static WebApplication MapGreetingEndpoints(this WebApplication app)
        {
            app.MapGet("/", GetHandler);

            return app;
        }

        private static string GetHandler([FromServices] ILogger<Program> logger)
        {
            // Log a message
            logger.LogInformation("Sending greeting");

            // Increment the custom counter
            MetricsCatalog.CountGreetings.Add(1);

            return "Hello World!";
        }
    }
}
