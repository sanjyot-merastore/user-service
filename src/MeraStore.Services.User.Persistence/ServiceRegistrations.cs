using MeraStore.Services.User.Application.Repositories;
using MeraStore.Services.User.Persistence.Repositories;
using MeraStore.Shared.Kernel.Persistence;
using MeraStore.Shared.Kernel.Persistence.Interfaces;
using MeraStore.Shared.Kernel.Persistence.Strategy;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MeraStore.Services.User.Persistence;

public static class ServiceRegistrations
{
  public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.TryAddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ICommitStrategy, DefaultCommitStrategy>();

    services.AddPersistence<AppDbContext>(op =>
      op.UseSqlServer(configuration.GetConnectionString("UserDb")));

    
    return services;
  }
}