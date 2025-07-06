using FluentAssertions;

using NetArchTest.Rules;

namespace MeraStore.Services.UserArchitectureTests;

public sealed class ApiTests : BaseTests
{
  [Fact]
  public void Api_Should_Not_Have_Direct_Access_To_Persistence()
  {
    var result = Types.InAssembly(ApiAssembly)
      .ShouldNot()
      .HaveDependencyOn(PersistenceAssemblyName)
      .GetResult();

    result.IsSuccessful.Should().BeTrue("API should not depend directly on the Persistence layer");
  }

  [Fact]
  public void Endpoints_Should_Implement_IEndpoint_Interface()
  {
    var types = Types.InAssembly(ApiAssembly)
      .That()
      .HaveNameEndingWith("Endpoint")
      .GetTypes();

    types
      .Should()
      .OnlyContain(t => t.GetInterfaces().Any(i => i.Name == "IEndpoint"),
        "All classes ending with 'Endpoint' should implement the IEndpoint interface");
  }

  [Fact]
  public void Endpoints_Should_Not_Have_Business_Logic()
  {
    var result = Types.InAssembly(ApiAssembly)
      .That()
      .HaveNameEndingWith("Endpoint")
      .Should()
      .HaveDependencyOn("MediatR")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Endpoints should delegate business logic via MediatR");
  }
}