using Wolverine_Rabbit;
using Wolverine_Rabbit.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults() // Aspire
       .AddCustomWolverine()
       .AddCustomHealthChecks()
       .AddCustomMetrics()
       .AddCustomHostedServices()
       .Build()
       .MapDefaultEndpoints() // Aspire
       .MapCustomHealthChecks()
       .MapGreetingEndpoints()
       .Run();

