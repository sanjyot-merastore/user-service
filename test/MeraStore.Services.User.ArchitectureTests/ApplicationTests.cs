using FluentAssertions;

using MediatR;

using NetArchTest.Rules;

namespace MeraStore.Services.UserArchitectureTests;

public sealed class ApplicationTests : BaseTests
{
  [Fact]
  public void Command_And_Query_Handlers_Should_Have_Proper_Naming()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .That()
      .ImplementInterface(typeof(IRequestHandler<>))
      .Or()
      .ImplementInterface(typeof(IRequestHandler<,>))
      .Should()
      .HaveNameEndingWith("Handler")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("All handlers should end with 'Handler'");
  }

  [Fact]
  public void Commands_And_Queries_Should_Have_Proper_Naming()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .That()
      .ImplementInterface(typeof(IRequest))
      .Or()
      .ImplementInterface(typeof(IRequest<>))
      .Should()
      .HaveNameEndingWith("Command")
      .Or()
      .HaveNameEndingWith("Query")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Requests should end with 'Command' or 'Query'");
  }

  [Fact]
  public void Handlers_Should_Reside_In_Handlers_Namespace()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .That()
      .ImplementInterface(typeof(IRequestHandler<>))
      .Or()
      .ImplementInterface(typeof(IRequestHandler<,>))
      .Should()
      .ResideInNamespace("MeraStore.Services.User.Application")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Handlers should reside in the Handlers namespace");
  }

  [Fact]
  public void Requests_Should_Reside_In_Commands_Or_Queries_Namespace()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .That()
      .ImplementInterface(typeof(IRequest))
      .Or()
      .ImplementInterface(typeof(IRequest<>))
      .Should()
      .ResideInNamespace("MeraStore.Services.User.Application.Features")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Commands and Queries should reside in proper namespaces");
  }

  [Fact]
  public void Endpoints_Should_Depend_On_Domain()
  {
    var result = Types.InAssembly(ApiAssembly)
      .That()
      .HaveNameEndingWith("Endpoint")
      .Should()
      .HaveDependencyOn("MediatR")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Application layer should depend on Domain layer.");
  }

}