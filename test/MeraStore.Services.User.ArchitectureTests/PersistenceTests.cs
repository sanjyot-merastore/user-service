using FluentAssertions;

using MeraStore.Shared.Kernel.Persistence.Interfaces;

using Microsoft.EntityFrameworkCore;

using NetArchTest.Rules;

namespace MeraStore.Services.UserArchitectureTests;
public sealed class PersistenceTests : BaseTests
{
  [Fact]
  public void Persistence_Should_Not_Have_Dependency_On_Api_Or_Application()
  {
    var result = Types.InAssembly(PersistenceAssembly)
      .ShouldNot()
      .HaveDependencyOnAny(ApiAssemblyName)
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Persistence should not depend on API or Application layers");
  }

  [Fact]
  public void Classes_Implementing_IUnitOfWork_Should_End_With_UnitOfWork()
  {
    var result = Types.InAssembly(PersistenceAssembly)
      .That()
      .ImplementInterface(typeof(IUnitOfWork)) // assuming IUnitOfWork exists
      .Should()
      .HaveNameEndingWith("UnitOfWork")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Unit of Work implementations should end with 'UnitOfWork'");
  }

  [Fact]
  public void DbContext_Should_Exist_And_Be_Suffixed()
  {
    var result = Types.InAssembly(PersistenceAssembly)
      .That()
      .AreClasses()
      .And()
      .Inherit(typeof(DbContext)) // assuming using Microsoft.EntityFrameworkCore
      .Should()
      .HaveNameEndingWith("DbContext")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("EF DbContext should be suffixed with 'DbContext'");
  }

  [Fact]
  public void Persistence_Should_Not_Depend_On_MediatR()
  {
    var result = Types.InAssembly(PersistenceAssembly)
      .ShouldNot()
      .HaveDependencyOn("MediatR")
      .GetResult();

    result.IsSuccessful.Should().BeTrue("Persistence should not depend on MediatR");
  }
}