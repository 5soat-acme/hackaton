using HT.Infra.Commons.EventBus;

namespace HT.Api.Commons.Config;

public static class EventBusConfig
{
    public static IServiceCollection AddEventBusConfig(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBus>();   

        return services;
    }

    public static WebApplication SubscribeEventHandlers(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var bus = services.GetRequiredService<IEventBus>();

        return app;
    }
}