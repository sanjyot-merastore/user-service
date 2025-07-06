using MeraStore.Services.User.Common;
using MeraStore.Shared.Kernel.WebApi;
using MeraStore.Shared.Kernel.WebApi.Interfaces;

using System.Reflection;

namespace MeraStore.Services.User.Api.Middlewares.Extensions
{
    /// <summary>
    /// Extension methods to register standard MeraStore middleware components.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds MeraStore tracing middleware.
        /// Enriches requests with correlation ID, request ID, and transaction ID.
        /// </summary>
        public static IApplicationBuilder UseMeraStoreTracing(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TracingMiddleware>(KeyStore.ServiceName);
        }

        /// <summary>
        /// Adds MeraStore error handling middleware.
        /// Handles exceptions and returns standardized error responses.
        /// </summary>
        public static IApplicationBuilder UseMeraStoreErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// Adds MeraStore API logging middleware.
        /// Logs request/response payloads with masking support.
        /// </summary>
        public static IApplicationBuilder UseMeraStoreLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>(new LoggingMiddlewareOptions
            {
              SkipPaths = ["/health", "/metrics", "/favicon.ico", "/"],
              SkipPathStartsWith = ["/swagger", "/_framework"],
              SkipMethods = ["OPTIONS", "HEAD"]
            });
        }

        /// <summary>
        /// Resolves all registered <see cref="IEndpoint"/> implementations from the service provider
        /// and invokes their <c>MapEndpoints</c> method to register their routes to the application.
        /// </summary>
        /// <param name="app">The endpoint route builder used to define API routes.</param>
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
          var endpointTypes = Assembly.GetExecutingAssembly()
            .ExportedTypes
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

          foreach (var endpoint in endpointTypes)
          {
            endpoint.MapEndpoints(app);
          }
        }
  }
}