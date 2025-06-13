using Wolverine_Rabbit.HostedServices;

internal static class HostedServicesExtensions
{
    public static WebApplicationBuilder AddCustomHostedServices(this WebApplicationBuilder builder)
    {
        builder.Services
               .AddHostedService<PingerService>();
        return builder;
    }


}