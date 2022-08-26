using Microsoft.OpenApi.Models;
using Croupier.Services;
using Croupier.Endpoints;
using Croupier.Workers;

public static class DependencyInitializer
{
    public static IServiceCollection AddDIServices(this IServiceCollection services)
    {
        services.AddTransient<IEndpoint, DeckEndpoint>();
        services.AddSingleton<ISessionManager, SessionManager> ();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CroupierAPI", Version = "v1" });
        });

        return services;
    }

    public static IApplicationBuilder UseOpenApi(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI(c => {
            c.RoutePrefix = ""; c.SwaggerEndpoint("/swagger/v1/swagger.json", "Croupier v0.1");
            });
        return builder;
    }

    public static WebApplication UseEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IEndpoint>().ToList();
        
        endpoints.ForEach(e => e.RegisterRoutes(app));

        return app;
    }

}
