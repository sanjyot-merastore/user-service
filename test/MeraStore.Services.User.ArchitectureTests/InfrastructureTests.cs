using FluentAssertions;
using MediatR;
using NetArchTest.Rules;

namespace MeraStore.Services.UserArchitectureTests;

public sealed class InfrastructureTests : BaseTests
{
  [Fact]
  public void Infrastructure_Should_Not_Contain_Application_Handlers()
  {
    var types = Types.InAssembly(InfrastructureAssembly)
      .That()
      .ImplementInterface(typeof(IRequestHandler<,>))
      .GetTypes();

    types.Should().BeEmpty("Handlers (IRequestHandler<,>) should not be implemented in the Infrastructure layer");
  }

  [Fact]
  public void Infrastructure_Should_Not_Reference_MediatR_Handlers()
  {
    var result = Types.InAssembly(InfrastructureAssembly)
      .ShouldNot()
      .HaveDependencyOn("MediatR")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Infra layer should not deal with MediatR handlers or use case logic");
  }

  [Fact]
  public void Infrastructure_Should_Contain_Services_With_Expected_Suffixes()
  {
    var result = Types.InAssembly(InfrastructureAssembly)
      .That()
      .AreClasses()
      .And()
      .AreNotStatic()
      .And()
      .ResideInNamespaceMatching(".*Infrastructure.*")
      .Should()
      .HaveNameEndingWith("Service")
      .Or()
      .HaveNameEndingWith("Client")
      .Or()
      .HaveNameEndingWith("Adapter")
      .Or()
      .HaveNameEndingWith("Provider")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Infra classes should follow naming convention for integrations and providers");
  }
}