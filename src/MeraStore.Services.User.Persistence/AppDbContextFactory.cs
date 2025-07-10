using MeraStore.Shared.Kernel.Persistence;

using Microsoft.EntityFrameworkCore;

namespace MeraStore.Services.User.Persistence;

public class AppDbContextFactory : BaseDbContextFactory<AppDbContext>
{
    protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options)
    {
        return new AppDbContext(options);
    }

    protected override string ConnectionStringName => "UserDb";
}