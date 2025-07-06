using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeraStore.Services.User.Persistence;

public static class ServiceRegistrations
{
  public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
  {

    return services;
  }
}