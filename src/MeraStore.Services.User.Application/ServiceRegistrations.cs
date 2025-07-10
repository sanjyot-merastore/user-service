using MediatR;

using MeraStore.Services.User.Application.Behaviours;
using MeraStore.Services.User.Application.Features.Health;
using MeraStore.Services.User.Application.Mappers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeraStore.Services.User.Application;

public static class ServiceRegistrations
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetHealthQuery).Assembly));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
    return services;
  }
}