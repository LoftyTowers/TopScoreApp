using Microsoft.Extensions.DependencyInjection;
using TopScore.Api.Services;
using TopScore.Core.Interfaces;

namespace TopScore.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<IWordValidator, WordValidator>();
        services.AddAutoMapper(typeof(Program));
        return services;
    }
}
