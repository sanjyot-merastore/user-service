using FluentAssertions;

using MeraStore.Services.User.Application.Features.Health;
using MeraStore.Services.User.Common;

namespace MeraStore.Services.User.UnitTests.Features.Health
{
  public class GetHealthQueryHandlerTests
  {
    [Fact]
    public async Task Handle_ShouldReturnHealthyResponse()
    {
      // Arrange
      var handler = new GetHealthQueryHandler();
      var query = new GetHealthQuery();

      // Act
      var result = await handler.Handle(query, CancellationToken.None);

      // Assert
      result.Should().NotBeNull();
      result.Status.Should().Be("Healthy");
      result.Service.Should().Be(KeyStore.ServiceName);
    }
  }
}