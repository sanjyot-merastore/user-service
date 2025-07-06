using MeraStore.Services.User.Application;
using MeraStore.Services.User.Common;
using MeraStore.Services.User.Common.Filters;
using MeraStore.Services.User.Domain.Entities;

using System.Reflection;

namespace MeraStore.Services.UserArchitectureTests;

public class BaseTests
{
  protected static readonly Assembly DomainAssembly = typeof(SampleEntity).Assembly;
  protected static readonly Assembly CommonAssembly = typeof(KeyStore).Assembly;
  protected static readonly Assembly ApplicationAssembly = typeof(ServiceRegistrations).Assembly;
  protected static readonly Assembly InfrastructureAssembly = typeof(MeraStore.Services.User.Infrastructure.ServiceRegistrations).Assembly;
  protected static readonly Assembly PersistenceAssembly = typeof(MeraStore.Services.User.Persistence.ServiceRegistrations).Assembly;
  protected static readonly Assembly ApiAssembly = typeof(MaskingFilterFactory).Assembly;


  protected static readonly string? DomainAssemblyName = "MeraStore.Services.User.Domain";
  protected static readonly string? CommonAssemblyName = CommonAssembly.GetName().Name;
  protected static readonly string? ApplicationAssemblyName = "MeraStore.Services.User.Application";
  protected static readonly string? InfrastructureAssemblyName = InfrastructureAssembly.GetName().Name;
  protected static readonly string? PersistenceAssemblyName = PersistenceAssembly.GetName().Name;
  protected static readonly string? ApiAssemblyName = ApiAssembly.GetName().Name;
}