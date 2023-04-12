using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posterr.Domain.Posts.Interfaces.Application;
using Posterr.Domain.Posts.Services;
using Posterr.Domain.Posts.Support.Options;

namespace Posterr.Domain.Posts.Support.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServiceCollection(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration == null) throw new ArgumentException(nameof(configuration));

        services.AddScoped<IPostApplication, PostApplicationService>();
        
        return services;
    }
}