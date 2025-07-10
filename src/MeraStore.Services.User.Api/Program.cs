using MeraStore.Services.User.Api.Middlewares.Extensions;
using MeraStore.Services.User.Common;
using MeraStore.Services.User.Infrastructure;
using MeraStore.Services.User.Persistence;
using MeraStore.Shared.Kernel.WebApi;
using MeraStore.Shared.Kernel.WebApi.Extensions;

using Microsoft.EntityFrameworkCore;

namespace MeraStore.Services.User.Api;

/// <summary>
/// 
/// </summary>
public class Program
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="args"></param>
  public static async Task Main(string[] args)
  {
    var builder = CreateWebApplicationBuilder(args);
    var app = await BuildWebApplication(builder);
    await app.RunAsync();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="args"></param>
  /// <returns></returns>
  public static WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddAuthorization();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    builder.AddApiServices(KeyStore.ServiceName, defaultLogging: true);
    builder.Services.AddInfrastructureServices(builder.Configuration);

    return builder;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public static async Task<WebApplication> BuildWebApplication(WebApplicationBuilder builder)
  {
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMeraStoreTracing();
    app.UseMeraStoreErrorHandling();
    app.UseCustomSwagger(KeyStore.ServiceName);
    app.UseMeraStoreLogging();
    app.UseHttpsRedirection();

    //Apply database migrations on startup with logging
    using (var scope = app.Services.CreateScope())
    {
      var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      var logger = app.Services.GetRequiredService<ILogger<Program>>();

      await RunMigrations(logger, dbContext);
    }

    app.MapEndpoints();
    app.MapControllers();

    return app;
  }

  static async Task RunMigrations(ILogger<Program> logger, AppDbContext appDbContext)
  {
    try
    {
      logger.LogInformation("Applying database migrations...");
      await appDbContext.Database.MigrateAsync();
      logger.LogInformation("✅ Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "❌ Error applying database migrations.");

    }
  }
}