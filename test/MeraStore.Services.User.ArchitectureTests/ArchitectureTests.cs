using FluentAssertions;

using MeraStore.Shared.Kernel.Core.Events;
using MeraStore.Shared.Kernel.Persistence.Interfaces;
using MeraStore.Shared.Kernel.WebApi.Interfaces;

using NetArchTest.Rules;

using System.Reflection;

namespace MeraStore.Services.UserArchitectureTests;

public class ArchitectureTests : BaseTests
{
  [Fact]
  public void Domain_Should_Not_Have_Any_Dependencies()
  {
    var result = Types.InAssembly(DomainAssembly)
        .ShouldNot()
        .HaveDependencyOnAny(
            ApplicationAssemblyName,
            InfrastructureAssemblyName,
            PersistenceAssemblyName,
            ApiAssemblyName)
        .GetResult();

    result.IsSuccessful.Should().BeTrue("Domain should not depend on other layers like Application, Infrastructure, etc.");
  }
  [Fact]
  public void Entities_Should_HavePrivateParameterlessConstructor()
  {
    var entityTypes = Types.InAssembly(DomainAssembly)
      .That()
      .Inherit(typeof(User.Domain.Entities.User))
      .GetTypes();
    var failingTypes = new List<Type>();
    foreach (var entityType in entityTypes)
    {
      var constructors = entityType.GetConstructors(
        BindingFlags.NonPublic |
        BindingFlags.Instance);
      if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
      {
        failingTypes.Add(entityType);
      }

      failingTypes.Should().BeEmpty();
    }
  }

  [Fact]
  public void Persistence_Should_Not_Have_Any_Dependencies()
  {
    var result = Types.InAssembly(PersistenceAssembly)
        .ShouldNot()
        .HaveDependencyOn(InfrastructureAssemblyName)
        .GetResult();

    result.IsSuccessful.Should().BeTrue("Persistence layer should not depend on Application layer.");
  }

  

  [Fact]
  public void Classes_Implementing_IEndpoint_Should_End_With_Endpoint()
  {
    var result = Types.InAssembly(ApiAssembly)
        .That()
        .ImplementInterface(typeof(IEndpoint))
        .Should()
        .HaveNameEndingWith("Endpoint")
        .GetResult();

    result.IsSuccessful.Should().BeTrue("All classes implementing IEndpoint should end with 'Endpoint'.");
  }

  [Fact]
  public void Classes_Implementing_IRepository_Should_End_With_Repository()
  {
    var result = Types.InAssembly(PersistenceAssembly)
        .That()
        .ImplementInterface(typeof(IRepository<>))
        .Should()
        .HaveNameEndingWith("Repository")
        .GetResult();

    result.IsSuccessful.Should().BeTrue("All classes implementing IRepository should end with 'Repository'.");
  }

  [Fact]
  public void Classes_Implementing_IDomainEvent_Should_End_With_DomainEvent()
  {
    var result = Types.InAssembly(DomainAssembly) // Usually DomainEvents live in Domain
        .That()
        .ImplementInterface(typeof(IDomainEvent))
        .Or()
        .Inherit(typeof(DomainEvent))
        .Should()
        .HaveNameEndingWith("DomainEvent")
        .GetResult();

    result.IsSuccessful.Should().BeTrue("All classes implementing IDomainEvent should end with 'DomainEvent'.");
  }
}