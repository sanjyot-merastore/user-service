using MeraStore.Services.User.Application;
using MeraStore.Services.User.Persistence;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeraStore.Services.User.Infrastructure;

public static class ServiceRegistrations
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddApplicationServices(configuration);
    services.AddPersistenceServices(configuration);
    return services;
  }
}