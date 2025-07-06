using MediatR;

namespace MeraStore.Services.User.Application.Features.Health;

/// <summary>
/// Represents a query to get the health status.
/// </summary>
public class GetHealthQuery : IRequest<HealthResponse>
{
}