using FluentAssertions;

using MeraStore.Services.User.IntegrationTests.Base;

using Microsoft.AspNetCore.Mvc.Testing;

using System.Net;

namespace MeraStore.Services.User.IntegrationTests;

public class HealthEndpointTests(WebApplicationFactory<Api.Program> factory)
  : BaseIntegrationTest(factory)
{
  [Fact]
  public async Task Get_Health_Should_Return_200_OK_And_HealthStatus()
  {
    var response = await Client.GetAsync("/health", CancellationToken.None);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var content = await response.Content.ReadAsStringAsync(CancellationToken.None);
    content.Should().Contain("Healthy");
  }
}
